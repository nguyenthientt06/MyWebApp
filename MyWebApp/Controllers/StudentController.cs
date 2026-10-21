using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyWebApp.Models;

namespace MyWebApp.Controllers
{
    public class StudentController : Controller
    {
        private List<Student> listStudents = new List<Student>();

        public StudentController()
        {
            // Tạo danh sách sinh viên với 4 dữ liệu mẫu
            listStudents = new List<Student>()
            {
                new Student() { Id = 101, Name = "Hải Nam", Branch = Branch.IT,
                    Gender = Gender.Male, IsRegular = true,
                    Address = "A1-2018", Email = "nam@g.com" },
                new Student() { Id = 102, Name = "Minh Tú", Branch = Branch.BE,
                    Gender = Gender.Female, IsRegular = true,
                    Address = "A1-2019", Email = "tu@g.com" },
                new Student() { Id = 103, Name = "Hoàng Phong", Branch = Branch.CE,
                    Gender = Gender.Male, IsRegular = false,
                    Address = "A1-2020", Email = "phong@g.com" },
                new Student() { Id = 104, Name = "Xuân Mai", Branch = Branch.EE,
                    Gender = Gender.Female, IsRegular = false,
                    Address = "A1-2021", Email = "mai@g.com" }
            };
        }

        // Cấu hình Route cho Student/Index thành Admin/Student/List
        [Route("Admin/Student/List", Name = "list")]
        public IActionResult Index()
        {
            return View(listStudents);
        }

        // Cấu hình Route cho Student/Create (HttpGet) thành Admin/Student/Add
        [HttpGet]
        [Route("Admin/Student/Add", Name = "add")]
        public IActionResult Create()
        {
            // Lấy danh sách các giá trị Gender để hiển thị radio button trên form
            ViewBag.AllGenders = Enum.GetValues(typeof(Gender)).Cast<Gender>().ToList();

            // Lấy danh sách các giá trị Branch để hiển thị select-option trên form
            ViewBag.AllBranches = new List<SelectListItem>()
            {
                new SelectListItem { Text = "IT", Value = "1" },
                new SelectListItem { Text = "BE", Value = "2" },
                new SelectListItem { Text = "CE", Value = "3" },
                new SelectListItem { Text = "EE", Value = "4" }
            };
            return View();
        }

        // Cấu hình Route cho phương thức POST khi submit form thêm mới
        [HttpPost]
        [Route("Admin/Student/Add")]
        public IActionResult Create(Student s)
        {
            // Tự động tăng ID cho sinh viên mới dựa vào ID của sinh viên cuối cùng trong danh sách
            s.Id = listStudents.Last<Student>().Id + 1;

            // Thêm sinh viên mới vào danh sách
            listStudents.Add(s);

            // Trả về lại trang Index kèm theo danh sách đã được cập nhật
            return View("Index", listStudents);
        }
        [HttpPost]
        [Route("Admin/Student/Add")]
        public async Task<IActionResult> Create(Student s, IFormFile avatarFile)
        {
            // Xử lý upload ảnh nếu người dùng có chọn file
            if (avatarFile != null && avatarFile.Length > 0)
            {
                // Lấy tên file gốc
                var fileName = Path.GetFileName(avatarFile.FileName);

                // Tạo đường dẫn lưu vào thư mục wwwroot/images
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                // Kiểm tra và tạo thư mục nếu chưa có
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.Exists(uploadsFolder);
                }

                var filePath = Path.Combine(uploadsFolder, fileName);

                // Lưu file vào thư mục
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await avatarFile.CopyToAsync(stream);
                }

                // Gán tên file vào thuộc tính Avatar của Student
                s.Avatar = fileName;
            }
            else
            {
                s.Avatar = "default.png"; // Ảnh mặc định nếu không chọn
            }

            s.Id = listStudents.Last<Student>().Id + 1;
            listStudents.Add(s);
            return View("Index", listStudents);
        }
    }
}