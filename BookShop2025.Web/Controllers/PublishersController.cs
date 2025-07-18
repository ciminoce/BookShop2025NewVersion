using AutoMapper;
using BookShop2025.Data.Interfaces;
using BookShop2025.Service.DTOs.Publisher;
using BookShop2025.Web.ViewModels.Publisher;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;
using X.PagedList.Extensions;

namespace BookShop2025.Web.Controllers
{
    public class PublishersController : Controller
    {
        private readonly IPublisherService _publisherService;
        private readonly ICountryService _countryService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public PublishersController(IPublisherService publisherService, IMapper mapper, ICountryService countryService, IWebHostEnvironment webHostEnvironment)
        {
            _publisherService = publisherService;
            _mapper = mapper;
            _countryService = countryService;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index(int? page, int? pageSize)
        {
            int pageNumber = page ?? 1;
            int registerPerPage = pageSize ?? 5;

            var publishersDto = _publisherService.GetAll();
            var pagedListDto = publishersDto.ToPagedList(pageNumber, registerPerPage);
            var viewModelList = _mapper.Map<List<PublisherListVm>>(pagedListDto); // Mapea en memoria

            var viewModelPagedList = new StaticPagedList<PublisherListVm>(
                viewModelList,
                pagedListDto.PageNumber,
                pagedListDto.PageSize,
                pagedListDto.TotalItemCount
            );

            return View(viewModelPagedList);
        }
        public IActionResult Upsert(int? id)
        {
            if (id is null || id == 0)
            {
                var publisherVm = new PublisherEditVm()
                {
                    Countries = GetCountries(),
                };
                return View(publisherVm);
            }
            try
            {
                PublisherEditDto? publisherDto = _publisherService.GetById(id.Value);
                if (publisherDto is null)
                {
                    return NotFound($"Publisher With Id {id} Not Found!!");
                }
                PublisherEditVm publisherVm = _mapper.Map<PublisherEditVm>(publisherDto);
                publisherVm.Countries = GetCountries();
                return View(publisherVm);
            }
            catch (Exception)
            {

                TempData["error"] = "Error while trying to get a publisher";
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(PublisherEditVm publisherVm, IFormFile? imageFile, bool removeImage = false)
        {
            if (ModelState.IsValid)
            {
                string wwwRoot = _webHostEnvironment.WebRootPath;
                string? fileAndExt = null;
                string? imagePathToDelete = null; // Ruta de la imagen si algo falla en la subida/DB
                string? oldImagePath = null;      // Ruta de la imagen anterior (si es una edición)

                // 1. Obtener la información del Publisher existente si es una edición
                // Esto es crucial para saber si ya hay una imagen y cuál es.
                PublisherEditDto? existingPublisherDto = null;
                if (publisherVm.PublisherId > 0) // Si el Id es mayor a 0, es una edición
                {
                    // Asumiendo que tienes un método para obtener el publisher por ID
                    // ¡Importante! Asegúrate de que este método devuelva el ImageUrl actual.
                    existingPublisherDto = _publisherService.GetById(publisherVm.PublisherId);
                    if (existingPublisherDto != null && !string.IsNullOrEmpty(existingPublisherDto.ImageUrl))
                    {
                        oldImagePath = Path.Combine(wwwRoot, existingPublisherDto.ImageUrl.TrimStart('\\'));
                    }
                }

                // 2. Lógica para subir una nueva imagen (si se proporciona)
                if (imageFile is not null)
                {
                    //Establezco nuevo nombre con un Guid
                    string fileName = Guid.NewGuid().ToString();
                    //Extraigo la extensión del archivo original
                    string extension = Path.GetExtension(imageFile.FileName);
                    //Establezco el nombre completo con extension
                    fileAndExt = $"{fileName}{extension}";
                    //Establezco toda la ruta del archivo a subir
                    string publisherImagesFolder = Path.Combine(wwwRoot, @"Images\Publishers");

                    // Asegurarse de que el directorio exista
                    if (!Directory.Exists(publisherImagesFolder))
                    {
                        Directory.CreateDirectory(publisherImagesFolder);
                    }
                    //Combino el directorio con el archivo a subir
                    //Por si lo tengo que borrar en la misma operación de guardado
                    imagePathToDelete = Path.Combine(publisherImagesFolder, fileAndExt); // Guarda la ruta para posible rollback

                    try
                    {
                        //Subo el archivo
                        using (var fileStream = new FileStream(imagePathToDelete, FileMode.Create))
                        {
                            imageFile.CopyTo(fileStream);
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, "Error trying to upload the image: " + ex.Message);
                        publisherVm.Countries = GetCountries();
                        return View(publisherVm);
                    }
                }

                // 3. Mapear ViewModel a DTO y asignar la URL de la imagen
                PublisherEditDto publisherDto = _mapper.Map<PublisherEditDto>(publisherVm);

                if (removeImage)
                {
                    // Si removeImage es true, se borra la imagen y se setea ImageUrl a null
                    publisherDto.ImageUrl = null;
                }
                else if (fileAndExt is not null)
                {
                    // Si se subió un nuevo archivo, usar la nueva URL
                    //al Dto para que lo pase al servicio y luego a la entidad
                    publisherDto.ImageUrl = $@"\Images\Publishers\{fileAndExt}";
                }
                else if (existingPublisherDto != null)
                {
                    // Si no se subió un nuevo archivo y no se pidió eliminar,
                    // mantener la URL de la imagen existente (solo si es una edición)
                    publisherDto.ImageUrl = existingPublisherDto.ImageUrl;
                }
                else
                {
                    // Caso de creación sin imagen o edición sin cambio de imagen y sin existente
                    publisherDto.ImageUrl = null;
                }

                // 4. Intentar guardar el Publisher en la base de datos
                try
                {
                    // Si es una edición, y se subió una nueva imagen o se marcó para eliminar,
                    // necesitamos actualizar el ID del DTO si no viene del mapeo
                    if (existingPublisherDto != null && publisherDto.PublisherId == 0)
                    {
                        publisherDto.PublisherId = existingPublisherDto.PublisherId;
                    }

                    if (_publisherService.Save(publisherDto, out var errors))
                    {
                        // Si la operación en la BD fue exitosa:

                        // a) Eliminar la imagen anterior si se subió una nueva o se marcó para eliminar
                        if (oldImagePath is not null && System.IO.File.Exists(oldImagePath))
                        {
                            // Solo eliminar si se subió una nueva imagen o si se pidió eliminar
                            if (fileAndExt is not null || removeImage)
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        TempData["success"] = "Registro actualizado exitosamente.";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        // Si falla el guardado en la BD, eliminar la imagen que acabamos de subir
                        if (imagePathToDelete is not null && System.IO.File.Exists(imagePathToDelete))
                        {
                            System.IO.File.Delete(imagePathToDelete);
                        }
                        if (errors != null && errors.Any())
                        {
                            ModelState.AddModelError(string.Empty, errors.First());
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Unexpeted error while saving the record.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Si ocurre una excepción al guardar en la BD, eliminar la imagen subida
                    if (imagePathToDelete is not null && System.IO.File.Exists(imagePathToDelete))
                    {
                        System.IO.File.Delete(imagePathToDelete);
                    }
                    ModelState.AddModelError(string.Empty, "¡Ups! Algo salió mal: " + ex.Message);
                }
            }

            // Si el ModelState no es válido o hay un error, recargar los países y volver a la vista
            publisherVm.Countries = GetCountries();
            return View(publisherVm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public IActionResult DeleteConfirm(int? id)
        {
            if (id is null || id == 0)
            {
                return NotFound();
            }
            try
            {
                PublisherEditDto? publisherDto = _publisherService.GetById(id.Value);
                if (publisherDto is null)
                {
                    return NotFound($"Publisher With Id {id} Not Found!!");
                }
                if (_publisherService.Remove(id.Value, out var errors))
                {
                    TempData["success"] = "Publisher Succesfully Removed";
                    return RedirectToAction("Index");
                }
                else
                {
                    PublisherListVm publisherVm = _mapper.Map<PublisherListVm>(publisherDto);
                    var paisDto = _countryService.GetById(publisherDto.CountryId);
                    publisherVm.CountryName = paisDto!.CountryName;

                    ModelState.AddModelError(string.Empty, errors.First());
                    return View(publisherVm);

                }
            }
            catch (Exception)
            {

                TempData["error"] = "Error while trying to get a publisher";
                return RedirectToAction("Index");
            }

        }

        private List<SelectListItem> GetCountries()
        {
            return _countryService.GetAll()
                .Select(c => new SelectListItem
                {
                    Value = c.CountryId.ToString(),
                    Text = c.CountryName
                }).ToList();

        }
    }
}
