using PagedList;
using ReviewFood.Areas.Admin.Controllers;
using ReviewFood.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReviewFood.Controllers
{
    public class DanhMucChaController : Controller
    {
        db_WebReviewFoodEntities db = new db_WebReviewFoodEntities();
        // GET: DanhMuc
        public ActionResult Index(int maDMCha,int page = 1, int id = 0)
        {

            var TinTucs = db.BaiViets.Where(p => p.IdDMCha == maDMCha).ToList();
            return View(TinTucs);
            //var TinTucs = db.BaiViets.Where(p => p.IdDMCha == maDMCha).ToList();

            //// Lấy đối tượng danh mục cha dựa trên maDMCha
            //var danhMucCha = db.DanhMucs.FirstOrDefault(dm => dm.Id == maDMCha);

            //if (danhMucCha == null)
            //{
            //    // Xử lý trường hợp không tìm thấy danh mục cha
            //    return RedirectToAction("Index", "Home");
            //}

            //ViewBag.Title = danhMucCha.TenDanhMuc;
            //return View(TinTucs);
        }
    }
}