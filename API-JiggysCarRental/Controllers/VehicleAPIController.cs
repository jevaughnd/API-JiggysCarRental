using API_JiggysCarRental.DATA;
using API_JiggysCarRental.MODELS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace API_JiggysCarRental.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _cxt;

        public VehicleAPIController(ApplicationDbContext context)
        {
            this._cxt = context;
        }


        //VEHICLE ENDPOINTS
        [HttpGet("Vehicle")]
        public IActionResult GetVehicles()
        {
            var vehicle = _cxt.Vehicles.Include(v => v.VehicleCategory).ToList();
            if (vehicle == null)
            {
                return BadRequest();
            }

            //Construct IMG Url
            var baseUrl = "https://localhost:7028/Images/";

            foreach(var img in vehicle)
            {
                img.FrontImgFilePath = baseUrl + img.FrontImgFilePath;
                img.SideImgFilePath= baseUrl + img.SideImgFilePath;
                img.InteriorImgFilePath= baseUrl + img.InteriorImgFilePath;
            }
            return Ok(vehicle);
        }


        //Find Individual Record By id
        [HttpGet("{id}")]
        public IActionResult GetVehicleById(int id)
        {
            var vehicle = _cxt.Vehicles.Include(c => c.VehicleCategory).FirstOrDefault (x => x.Id == id);

            if(vehicle == null)
            {
                return NotFound();
            }

            //Gets Saved Images by Id, to show in details page
            var baseUrl = "https://localhost:7028/Images/";
            vehicle.FrontImgFilePath = baseUrl + vehicle.FrontImgFilePath;
            vehicle.SideImgFilePath = baseUrl + vehicle.SideImgFilePath;
            vehicle.InteriorImgFilePath = baseUrl + vehicle.InteriorImgFilePath;

            return Ok(vehicle);
        }




        //Create Record
        [HttpPost("VehiclePost")] // not used
        public IActionResult CreateVehicle([FromBody] Vehicle values)
        {
            _cxt.Vehicles.Add(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetVehicleById), new { id = values.Id }, values);
        }



        //Edit Record 
        [HttpPut("VehiclePut")]  //not being used
        public IActionResult UpdateVehicle([FromBody] Vehicle vehicleValues)
        {
            if(vehicleValues.Id==null)
            {
                return NotFound();
            }
            _cxt.Vehicles.Update(vehicleValues);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetVehicleById), new {id = vehicleValues.Id}, vehicleValues);

        }



        //Delete Individual Record By Id
        [HttpDelete("{id}")]
        public IActionResult DeleteVehicleById(int id)
        {
            var vehicle = _cxt.Vehicles.Include(c => c.VehicleCategory).FirstOrDefault(x => x.Id == id);

            if(vehicle == null) { return NotFound(); }

            _cxt.Vehicles.Remove(vehicle);
            _cxt.SaveChanges();
            return Ok(vehicle);
        }



                                  // CREATE DTO File //
        //----------------------------------------------------------------------
        //To Create ImageFile   
        [HttpPost("filePost")]
        public async Task<IActionResult> Create([FromForm] VehicleCreateDTO model)
        {
            if (ModelState.IsValid)
            {   //take file from DTO

                var frontImageFile = model.FrontImgFile;
                var sideImageFile = model.SideImgFile;
                var interiorImageFile = model.InteriorImgFile;



                // 1st IMAGE
                if (frontImageFile != null && frontImageFile.Length > 0) //1st IMAGE
                {
                    //generate a unique file name
                    var uniqueFileName1 = Guid.NewGuid() + "_" + frontImageFile.FileName;

                    //define the final file path on the API server
                    var apiFilePath1 = Path.Combine("api", "server", "VehicleImgFileUploads", uniqueFileName1);

                    //Save file to server
                    using (var stream = new FileStream(apiFilePath1, FileMode.Create))
                    {
                        await frontImageFile.CopyToAsync(stream);
                    }


                    // 2nd IMAGE
                    if (sideImageFile != null && sideImageFile.Length > 0) // 2nd IMAGE
                    {
                        //generate
                        var uniqueFileName2 = Guid.NewGuid() + "_" + sideImageFile.FileName;
                        //define
                        var apiFilePath2 = Path.Combine("api", "server", "VehicleImgFileUploads", uniqueFileName2);

                        //save
                        using (var stream = new FileStream(apiFilePath2, FileMode.Create))
                        {
                            await sideImageFile.CopyToAsync(stream);
                        }

                        // 3rd IMAGE
                        if (interiorImageFile != null && interiorImageFile.Length > 0) // 3rd IMAGE
                        {
                            //genrate
                            var uniqueFileName3 = Guid.NewGuid()+"_" + interiorImageFile.FileName;
                            //define
                            var apiFilePath3 = Path.Combine("api", "server", "VehicleImgFileUploads", uniqueFileName3);

                            //save
                            using (var stream = new FileStream(apiFilePath3, FileMode.Create))
                            {
                                await interiorImageFile.CopyToAsync(stream);
                            }

                            //Store the file path in the database along with other Vehicle details
                            var vehicle = new Vehicle
                            {
                                VehicleCategoryId = model.VehicleCategoryId,
                                VehicleName = model.VehicleName,
                                VehicleDescription = model.VehicleDescription,
                                LicencePlateData = model.LicencePlateData,

                                FrontImgFilePath = apiFilePath1 != String.Empty ? apiFilePath1 : "",
                                SideImgFilePath = apiFilePath2 != String.Empty ? apiFilePath2 : "",
                                InteriorImgFilePath = apiFilePath3!= String.Empty ? apiFilePath3 :"",

                                NumberOf_Passengers = model.NumberOf_Passengers,
                                NumberOf_LargeBags = model.NumberOf_LargeBags,
                                Has_AirConditioner = model.Has_AirConditioner,
                                Has_Radio_CDPlayer = model.Has_Radio_CDPlayer,
                                Has_PowerSteering_Windows = model.Has_PowerSteering_Windows,
                                Has_AutomaticTransmission = model.Has_AutomaticTransmission,
                            };

                            //Save The Vehicle to the database
                            _cxt.Vehicles.Add(vehicle);
                            await _cxt.SaveChangesAsync();
                            return CreatedAtAction(nameof(GetVehicleById), new { id = vehicle.Id }, vehicle);

                        }//3rd

                    }//2nd

                }//1st

            }
            return BadRequest(ModelState);
        }


        //-----------------------------------
        //Retrieve a link to the uploaded file
        [HttpGet("files/{fileName}")]
        public IActionResult GetFile(string fileName)
        {
            // Construct the full path to the file based on the provided 'fileName'
            string filePath = Path.Combine("api", "server", "VehicleImgFileUploads", fileName);


            // Check if the file exists
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(); // Or handle the case where the file doesn't exist
            }

            // Determine the content type based on the file's extension
            string contentType = GetContentType(fileName);


            // Return the image file as a FileStreamResult with the appropriate content type
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return new FileStreamResult(fileStream, contentType); // Adjust the content type as needed

        }
        private string GetContentType(string fileName)
        {
            // Determine the content type based on the file's extension
            string ext = Path.GetExtension(fileName).ToLowerInvariant();
            switch (ext)
            {
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".pdf":
                    return "application/pdf";
                default:
                    return "application/octet-stream"; // Default to binary data
            }
        }









        //---------------------------------------------------------------------//
        // EDIT: DTO FILE //

        [HttpPut("filePut/{id}")] 
        public async Task<IActionResult> EditFile(int id, [FromForm] VehicleCreateDTO model)
        {
            if (ModelState.IsValid)
            {
                var existingVehicle = await _cxt.Vehicles.FindAsync(id);

                if (existingVehicle == null)
                {
                    return NotFound();
                }



                // Check if a new front image is provided
                if (model.FrontImgFile != null && model.FrontImgFile.Length > 0)
                {
                    // Generate a unique file name for the front image
                    var uniqueFrontFileName = Guid.NewGuid() + "_" + model.FrontImgFile.FileName;

                    // Define the final file path on the server
                    var apiFrontFilePath = Path.Combine("api", "server", "Updated-Vehicle-Imgs", uniqueFrontFileName);

                    // Save the new front image to the server, overwriting the existing front image
                    using (var stream = new FileStream(apiFrontFilePath, FileMode.Create))
                    {
                        await model.FrontImgFile.CopyToAsync(stream);
                    }

                    // Update the front image file path in the database
                    existingVehicle.FrontImgFilePath = apiFrontFilePath;
                }


                // Repeat the same process for side and interior images (if provided)

                // Check if a new side image is provided
                if (model.SideImgFile != null && model.SideImgFile.Length > 0)
                {
                    // Generate
                    var uniqueSideFileName = Guid.NewGuid() + "_" + model.SideImgFile.FileName;
                    // Define the final file path 
                    var apiSideFilePath = Path.Combine("api", "server", "Updated-Vehicle-Imgs", uniqueSideFileName);
                    // Save & overwrite
                    using (var stream = new FileStream(apiSideFilePath, FileMode.Create))
                    {
                        await model.SideImgFile.CopyToAsync(stream);
                    }
                    // Update
                    existingVehicle.SideImgFilePath = apiSideFilePath;
                }


                // Check if a new interior image is provided
                if (model.InteriorImgFile != null && model.InteriorImgFile.Length > 0)
                {

                    // Generate 
                    var uniqueInteriorFileName = Guid.NewGuid() + "_" + model.InteriorImgFile.FileName;
                    // Define the final file path 
                    var apiInteriorFilePath = Path.Combine("api", "server", "Updated-Vehicle-Imgs", uniqueInteriorFileName);
                    // Save & overwrite
                    using (var stream = new FileStream(apiInteriorFilePath, FileMode.Create))
                    {
                        await model.InteriorImgFile.CopyToAsync(stream);
                    }
                    // Update 
                    existingVehicle.InteriorImgFilePath = apiInteriorFilePath;
                }


                //Update other properties of the Vehicle entity based on the model
                existingVehicle.VehicleCategoryId = model.VehicleCategoryId;
                existingVehicle.VehicleName = model.VehicleName;
                existingVehicle.VehicleDescription = model.VehicleDescription;
                existingVehicle.LicencePlateData = model.LicencePlateData;
                existingVehicle.NumberOf_Passengers = model.NumberOf_Passengers;
                existingVehicle.NumberOf_LargeBags = model.NumberOf_LargeBags;
                existingVehicle.Has_AirConditioner = model.Has_AirConditioner;
                existingVehicle.Has_Radio_CDPlayer = model.Has_Radio_CDPlayer;
                existingVehicle.Has_PowerSteering_Windows = model.Has_PowerSteering_Windows;
                existingVehicle.Has_AutomaticTransmission = model.Has_AutomaticTransmission;


                // Save the changes to the database
                _cxt.Vehicles.Update(existingVehicle); 
                _cxt.SaveChanges();
                return CreatedAtAction(nameof(GetVehicleById), new { id = existingVehicle.Id }, existingVehicle);
            }

            return BadRequest(ModelState);
        }





        //--------------------------------------------------------------------
        //DELETE: RECORD & IMG FILES / Remove files from folder path
        ///The API for delete file and record works, but the frontend dosent as yet
        [HttpDelete("deleteVehicle/{id}")]
        public IActionResult DeleteVehicle(int id)
        {
            try
            {
                var existingVehicle = _cxt.Vehicles.Find(id);

                if (existingVehicle == null)
                {
                    return NotFound();
                }

                // Delete the img files
                DeleteFile(existingVehicle.FrontImgFilePath);
                DeleteFile(existingVehicle.SideImgFilePath);
                DeleteFile(existingVehicle.InteriorImgFilePath);

                // Remove the vehicle record from the database
                _cxt.Vehicles.Remove(existingVehicle);
                _cxt.SaveChanges();

                return Ok("Vehicle and Img files deleted successfully.");
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                return StatusCode(500, "Internal server error.");
            }
        }

        private void DeleteFile(string filePath)
        {
            // file deletion logic
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }


        //------------------------------------------------------------------








        //=========================================================================================================================//
        //VEHICLE CATEGORY ENDPOINTS


        [HttpGet("VehicleCategory")]
        public IActionResult GetCategory()
        {
            var category = _cxt.VehicleCategories.ToList();
            if(category ==null)
            {
                return BadRequest();
            }
            return Ok(category);

        }



        [HttpGet]
        [Route("VehicleCategory/{Id}")]
        public IActionResult GetCategoryById(int id)
        {
            var category = _cxt.VehicleCategories.FirstOrDefault(c => c.Id == id);
            if(category == null)
            {
                return NotFound();

            }
            return Ok(category);
        }


        [HttpPost("VehicleCategoryPost")]

        public IActionResult CreateCategory([FromBody] VehicleCategory values )
        {
            _cxt.VehicleCategories.Add(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetCategoryById), new {id = values.Id}, values);
        }


        [HttpPut("VehicleCategoryPut")]
        public IActionResult UpdateCategory(int id, [FromBody] VehicleCategory values)
        {
            var category = _cxt.VehicleCategories.FirstOrDefault(c => c.Id==id );
            if (category == null)
            {
                return NotFound();
            }
            _cxt.VehicleCategories.Update(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetCategoryById), new {id = values.Id},values );
        }
    }
}
