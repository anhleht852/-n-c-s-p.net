using ReviewFood.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReviewFood.Areas.Admin.Controllers
{
    public class BaiVietController : Controller
    {
        db_WebReviewFoodEntities db = new db_WebReviewFoodEntities();
        // GET: Admin/BaiViet
        public ActionResult Index(string IdDanhMuc, string keyword)
        {
            int idDM = 0;
            if (IdDanhMuc != null) idDM = int.Parse(IdDanhMuc);
            ViewBag.keyword = keyword;
            if (IdDanhMuc != null)
                ViewBag.IdDanhMuc = int.Parse(IdDanhMuc);
            else ViewBag.IdDanhMuc = IdDanhMuc;
            ViewBag.DanhMucs = db.DanhMucs.ToList();
            List<BaiViet> baiViets = new List<BaiViet>();

            if (idDM == 0 && keyword == "" || idDM == 0 && keyword == null)
                baiViets = db.BaiViets.ToList();
            else if (idDM != 0 && keyword == "")
                baiViets = db.BaiViets.Where(dm => dm.IdDanhMuc.Equals(idDM)).ToList();
            else if (idDM == 0 && keyword != "")
                baiViets = db.BaiViets.Where(dm => dm.TieuDe.Contains(keyword)).ToList();
            else
                baiViets = db.BaiViets.Where(dm => dm.TieuDe.Contains(keyword) && dm.IdDanhMuc.Equals(idDM)).ToList();
       
            return View(baiViets);
        }

        // GET: Admin/BaiViet/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Admin/BaiViet/Create
        public ActionResult Create()
        {
            ViewBag.DanhMucs = db.DanhMucs.ToList();
            ViewBag.DanhMucChas = db.DanhMucChas.ToList();
            return View();
        }

        // POST: Admin/BaiViet/Create
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BaiViet baiViet, HttpPostedFileBase img)
        {
            try
            {
                if (img != null)
                {
                    string fileId = Guid.NewGuid().ToString().Replace("-", "");
                    string extension = Path.GetExtension(img.FileName);
 //kiểm tra xem tệp tin img có tồn tại hay không. Nếu có, tạo một fileId duy nhất bằng cách sử dụng
 //Guid.NewGuid().ToString().Replace("-", ""), và lấy phần mở rộng của tệp tin (extension) bằng Path.GetExtension(img.FileName).
                    if (extensionFile(extension))
                    {
                        string filename = fileId + extension;
                        string filePath = Path.Combine(Server.MapPath("~/Assets/Image"), filename);
                        img.SaveAs(filePath);
                        baiViet.HinhAnh = filename;
                    }

                    else
                    {
                        ViewBag.Error = "File không đúng định dạng, hãy điền lại dữ liệu";
                        return View(baiViet);
                    }
                }

                baiViet.NgayTao = baiViet.NgaySua = DateTime.Now;
                db.BaiViets.Add(baiViet);
                db.SaveChanges();

                ViewBag.Done = "Thêm bài viết thành công";
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            ViewBag.DanhMucs = db.DanhMucs.ToList();
            ViewBag.DanhMucChas = db.DanhMucChas.ToList();

            return View(baiViet);
        }

        public ActionResult Edit(int id)
        {
            var baiViet = db.BaiViets.FirstOrDefault(tt => tt.Id == id);
            if (baiViet == null)
                return RedirectToAction("Index", "BaiViet");

            ViewBag.DanhMucs = db.DanhMucs.ToList();
            ViewBag.DanhMucChas = db.DanhMucChas.ToList();

            ViewBag.CurrentDanhMucId = baiViet.IdDanhMuc;
            ViewBag.CurrentDanhMucTen = db.DanhMucs.FirstOrDefault(dm => dm.Id == baiViet.IdDanhMuc)?.TenDanhMuc;
            ViewBag.CurrentDanhMucChaId = baiViet.IdDMCha;
            ViewBag.CurrentDanhMucChaTen = db.DanhMucChas.FirstOrDefault(dmc => dmc.MaDMCha == baiViet.IdDMCha)?.TenDM;

            return View(baiViet);
        }


        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BaiViet baiViet, HttpPostedFileBase img)
        {
            try
            {
                if (img != null && extensionFile(Path.GetExtension(img.FileName)))
                {
                    string filename = Path.GetFileNameWithoutExtension(img.FileName);
                    string _filename = filename + Path.GetExtension(img.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/Assets/Image"), _filename);
                    img.SaveAs(filePath);
                    baiViet.HinhAnh = _filename;
                }
                else if (img != null)
                {
                    TempData["messageError"] = "File không đúng định dạng, hãy điền lại dữ liệu";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            var existingBaiViet = db.BaiViets.FirstOrDefault(tt => tt.Id == baiViet.Id);
            if (existingBaiViet != null)
            {
                existingBaiViet.TieuDe = baiViet.TieuDe;
                existingBaiViet.NoiDung = baiViet.NoiDung;
                if (img == null)
                {
                    existingBaiViet.HinhAnh = existingBaiViet.HinhAnh; // Giữ nguyên hình ảnh hiện tại nếu không cập nhật
                }
                else
                {
                    existingBaiViet.HinhAnh = baiViet.HinhAnh;
                }
                existingBaiViet.IdDanhMuc = baiViet.IdDanhMuc;
                existingBaiViet.IdDMCha = baiViet.IdDMCha;
                existingBaiViet.NgaySua = DateTime.Now;
                existingBaiViet.TrangThai = baiViet.TrangThai;
                db.SaveChanges();

                ViewBag.Done = "Sửa bài viết thành công";
            }

            ViewBag.DanhMucs = db.DanhMucs.ToList();
            ViewBag.DanhMucChas = db.DanhMucChas.ToList();

            ViewBag.CurrentDanhMucId = existingBaiViet.IdDanhMuc;
            ViewBag.CurrentDanhMucTen = db.DanhMucs.FirstOrDefault(dm => dm.Id == existingBaiViet.IdDanhMuc)?.TenDanhMuc;
            ViewBag.CurrentDanhMucChaId = existingBaiViet.IdDMCha;
            ViewBag.CurrentDanhMucChaTen = db.DanhMucChas.FirstOrDefault(dmc => dmc.MaDMCha == existingBaiViet.IdDMCha)?.TenDM;

            return View(baiViet);
        }

        // GET: Admin/BaiViet/Delete/5
        public ActionResult Delete(int id)
        {
            var data = db.BaiViets.Find(id);
            if (data == null)
                return RedirectToAction("Index", "DanhMuc");
                return View(data);
        }

        // POST: Admin/BaiViet/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, BaiViet baiviet)
        {
            try
            {
                BaiViet dbEntity = db.BaiViets.Find(id);
                db.BaiViets.Remove(dbEntity);
                db.SaveChanges();
                TempData["Done"] = "Xóa danh mục thành công";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("Index", "BaiViet");
        }


        public bool extensionFile(string extension)
        {
            string[] validExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".veg" };
            return validExtensions.Contains(extension.ToLower());
        }




        ///

        //

        public ActionResult TinChuaDuyet(string IdDanhMuc, string keyword)
        {
            int idDM = 0;
            if (IdDanhMuc != null) idDM = int.Parse(IdDanhMuc);
            ViewBag.keyword = keyword;
            if (IdDanhMuc != null)
                ViewBag.IdDanhMuc = int.Parse(IdDanhMuc);
            else ViewBag.IdDanhMuc = IdDanhMuc;
            ViewBag.DanhMucs = db.DanhMucs.ToList();
            List<BaiViet> baiViets = new List<BaiViet>();
            if (idDM == 0 && keyword == "" || idDM == 0 && keyword == null)
                baiViets = db.BaiViets.Where(tt => tt.TrangThai == false).ToList();
            else if (idDM != 0 && keyword == "")
                baiViets = db.BaiViets.Where(dm => dm.IdDanhMuc.Equals(idDM) && dm.IdDanhMuc.Equals(idDM) && dm.TrangThai == false).ToList();
            else if (idDM == 0 && keyword != "")
                baiViets = db.BaiViets.Where(dm => dm.TieuDe.Contains(keyword) && dm.IdDanhMuc.Equals(idDM) && dm.TrangThai == false).ToList();
            else
                //baiViets = db.BaiViets.Where(dm => dm.TieuDe.Contains(keyword) && dm.IdDanhMuc.Equals(idDM)).ToList();
                baiViets = db.BaiViets.Where(dm => dm.TieuDe.Contains(keyword) && dm.IdDanhMuc.Equals(idDM) && dm.TrangThai == false).ToList();
            if (TempData["Done"] != null || TempData["Error"] != null)
            {
                ViewBag.Done = TempData["Done"];
                ViewBag.Error = TempData["Error"];
                TempData.Remove("Done");
                TempData.Remove("Error");
            }
            return View(baiViets);
        }
   
        public ActionResult EditDuyetBai(int id)
        {
            var data = db.BaiViets.Find(id);
            if (data == null)
            {
                return RedirectToAction("Index", "BaiViet");
            }
            ViewBag.DanhMucs = db.DanhMucs.ToList();
            return View(data);
        }

        // POST: Admin/BaiViet/Edit/5
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult EditDuyetBai(BaiViet tinTucs, HttpPostedFileBase img)
        {
            var query = (from tt in db.BaiViets
                         where tt.Id == tinTucs.Id
                         select tt).Take(1);
            foreach (BaiViet tt in query)
            {
                tt.TieuDe = tinTucs.TieuDe;
                tt.NoiDung = tinTucs.NoiDung;
                if (tinTucs.HinhAnh != null)
                    tt.HinhAnh = tinTucs.HinhAnh;
                else tinTucs.HinhAnh = tt.HinhAnh;
                tt.IdDanhMuc = tinTucs.IdDanhMuc;
                tt.NgaySua = DateTime.Now;
                tt.TrangThai = tinTucs.TrangThai;
            };
            db.SaveChanges();
            ViewBag.Done = "Sửa Trạng Thái thành công";
            ViewBag.DanhMucs = db.DanhMucs.ToList();
            return View(tinTucs);
        }
       
    }
}