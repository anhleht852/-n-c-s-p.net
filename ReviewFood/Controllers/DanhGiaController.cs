using ReviewFood.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReviewFood.Controllers
{
    public class DanhGiaController : Controller
    {
        db_WebReviewFoodEntities db = new db_WebReviewFoodEntities();
        // GET: DanhGia
        public ActionResult Create(string cMessage, int MaTinTuc)
        {
            var session = Session["TaiKhoan"];
            if (session == null)
            {
                TempData["Error"] = "Bạn phải đăng nhập";
                return Redirect("/BaiViet/Index/" + MaTinTuc);
            };
            string data = Session["TaiKhoan"].ToString();
            string[] Account = new string[3];// khởi tạo một mảng có tên là Account với kích thước là 3 phần tử.
            Account = (data != null) ? data.Split(',') : Account;
            //để tách chuỗi data thành các phần tử riêng biệt dựa trên ký tự phân cách là dấu phẩy (',').
            DanhGia cmt = new DanhGia();
            cmt.NoiDung = cMessage;
            cmt.IdTinTuc = MaTinTuc;

            cmt.IdTaiKhoan = int.Parse(Account[2]);//lấy giá trị từ phần tử thứ 2 trong mảng Account, chuyển đổi nó thành số nguyên và gán cho thuộc tính IdTaiKhoan của đối tượng cmt.
            cmt.NgayTao = DateTime.Now;
            cmt.NgaySua = DateTime.Now;
            cmt.TrangThai = true;
            db.DanhGias.Add(cmt);
            db.SaveChanges();
            return Redirect("/BaiViet/Index/" + MaTinTuc);
        }
    }
}