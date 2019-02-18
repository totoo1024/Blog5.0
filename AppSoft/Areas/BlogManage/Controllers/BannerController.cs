using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Entities.Dtos;
using Microsoft.AspNetCore.Mvc;
using App.IServices;
using App.Entities;
using System.ComponentModel;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using AppSoft.Filter;

namespace AppSoft.Areas.BlogManage.Controllers
{
    [Area("BlogManage")]
    public class BannerController : BaseControler
    {
        private readonly IBannerInfoLogic _bannerInfoLogic;
        public BannerController(IBannerInfoLogic bannerInfoLogic)
        {
            _bannerInfoLogic = bannerInfoLogic;
        }
        [HttpPost]
        public IActionResult Index(QueryDto query)
        {
            return Json(_bannerInfoLogic.QueryableByPage(query), "yyyy-MM-dd HH:mm:ss");
        }

        [HttpPost]
        [Description("新增编辑轮播图")]

        public IActionResult Form(BannerInfo banner)
        {
            return Json(_bannerInfoLogic.Save(banner));
        }

        [AllowAccessFilter]
        [Description("轮播图详情")]
        public IActionResult Detail(string key)
        {
            return Json(_bannerInfoLogic.FindEntity(key));
        }

        /// <summary>
        /// 上传banner图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAccessFilter]
        public IActionResult UploadImg([FromServices]IHostingEnvironment env)
        {
            OperateResult<string> result = new OperateResult<string>();
            try
            {
                var file = Request.Form.Files[0];
                string fileName = Path.GetFileName(file.FileName);
                string fileExtension = Path.GetExtension(fileName);
                if (".jpg.png.gif.jpeg".Contains(fileExtension))
                {
                    string savePath = env.WebRootPath + "/Uploads/Resource";
                    if (!Directory.Exists(savePath))
                    {
                        Directory.CreateDirectory(savePath);
                    }
                    string name = DateTime.Now.ToString("yyyyMMddHHmmss") + fileExtension;
                    string imgPath = savePath + "/" + name;
                    using (FileStream fs = System.IO.File.Create(imgPath))
                    {
                        file.CopyTo(fs);
                        fs.Flush();
                    }
                    result.Data = "/Uploads/Resource/" + name;
                    result.Status = ResultStatus.Success;
                }
                else
                {
                    result.Message = "上传图片格式必须为jpg|png|gif|jpeg";
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Json(result);
        }

        [HttpPost]
        [Description("删除轮播图")]
        public IActionResult Delete(string key)
        {
            return Json(_bannerInfoLogic.UpdateRemoveCache(b => new BannerInfo() { DeleteMark = true }, c => c.BannerId == key));
        }
    }
}