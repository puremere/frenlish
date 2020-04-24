using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using frenlish.ViewModel;
using frenlish.Models;
using System.IO;

namespace frenlish.Controllers
{
    public class AdminController : Controller
    {
        dbManger manager = new dbManger();
        public static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }


        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult document()
        {
            //string query = "";
            //documentVM model = new documentVM()
            //{
            //    coursList = manager.getCourseList(query).ToList()
            //};

            return View();
        }

        #region blogsection
        public ActionResult blog()
        {

            Session["imageListAdd"] = "";
            Session["imageListEdit"] = "";
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            //using (WebClient client = new WebClient())
            //{

            //    var collection = new NameValueCollection();
            //    collection.Add("device", device);
            //    collection.Add("code", code);
            //    collection.Add("servername", servername);
            //    byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getDataCatArticle.php", collection);

            //    result = System.Text.Encoding.UTF8.GetString(response);
            //}

            List<articleCatVM> log2 = manager.getCatArticleList().Select(i => new articleCatVM
            {
                articleCatID = i.articleCatID,
                image = i.image,
                title = i.title,
                titleEn = i.titleEn
            }).ToList();
            List<articleVM> log = manager.getArticleList().Select(i => new articleVM
            {
                arcticleID = i.arcticleID,
                articleCatID = i.articleCatID,
                content = i.content,
                title = i.title,
                date = i.date,
                hashtags = i.hashtags,
                image = i.image,
                link = i.link,
                view = i.view,
                writer = i.writer,
                catname = i.articleCat.title


            }).ToList();

            ViewModel.AdminBlogVM model = new ViewModel.AdminBlogVM()
            {
                Articlelist = log,
                Catlist = log2
            };

            return View(model);
        }
        public List<article> getArticleList(string id, string search)
        {
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            //using (WebClient client = new WebClient())
            //{

            //    var collection = new NameValueCollection();
            //    collection.Add("device", device);
            //    collection.Add("code", code);
            //    collection.Add("id", id);
            //    collection.Add("search", search);
            //    collection.Add("servername", servername);
            //    byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/getDataCatArticlesList.php", collection);

            //    result = System.Text.Encoding.UTF8.GetString(response);
            //}
            List<article> model = new List<article>();
            return model;
        }
        public ActionResult setNewArticle(ViewModel.newArcticelVM model)
        {
            if (model.description.Contains("script"))
            {
                return RedirectToAction("blog", "Admin");
            }
            model.tag = model.tag.Replace(",", "-");

            string ss = Session["imageListAdd"] as string;
            ss = ss.Substring(0, ss.Length - 1);
            List<string> imageList = ss.Split(',').ToList();
            string imagename = "";
            if (imageList != null)
            {
                imagename = imageList[0];
            }
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");

            article newArticle = new article()
            {
                articleCatID = model.catList,
                content = model.description,
                hashtags = model.tag,
                date = DateTime.Now,
                image = imagename,
                title = model.title,
                writer = "مدیر",

            };
            dbRespose res = manager.addArticle(newArticle);
            if (res.status == 200)
            {
                return RedirectToAction("blog");
            }
            else
            {
                return RedirectToAction("blog");
            }

        }
        [HttpPost]
        public ActionResult setNewCatArticle(string image, string title)
        {
            string pathString = "~/images/panelimages";
            if (!Directory.Exists(Server.MapPath(pathString)))
            {
                DirectoryInfo di = Directory.CreateDirectory(Server.MapPath(pathString));
            }
            string imagename = "";
            for (int i = 0; i < Request.Files.Count; i++)
            {

                HttpPostedFileBase hpf = Request.Files[i];

                if (hpf.ContentLength == 0)
                    continue;
                imagename = RandomString(7) + ".jpg";
                string savedFileName = Path.Combine(Server.MapPath(pathString), imagename);
                hpf.SaveAs(savedFileName);
            }
            string device = RandomString(10);
            //string code = MD5Hash(device + "ncase8934f49909");
            articleCat NewCat = new articleCat()
            {
                image = imagename,
                title = title,
                titleEn = RandomString(5),

            };

            dbRespose res = manager.addArticleCat(NewCat);
            if (res.status == 200)
            {
                return RedirectToAction("blog");

            }
            else
            {
                return RedirectToAction("blog");

            }

        }
        public ActionResult updateArticle(ViewModel.updateArticleVM model)
        {
            model.tagupdate = model.tagupdate.Replace(",", "-");

            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");

            string ss = Session["imageListEdit"] as string;
            // ss = ss + filename;
            ss = ss.Substring(0, ss.Length - 1);
            List<string> imaglist = ss.Split(',').ToList();

            article updatedArticle = new article()
            {
                articleCat = manager.getCatArtice(model.catListupdate),
                content = model.descriptionupdate,
                hashtags = model.tagupdate,
                date = DateTime.Now,
                image = imaglist[0],
                arcticleID = Int32.Parse(model.IDupdate),
            };
            dbRespose res = manager.updataArtice(updatedArticle);
            if (res.status == 200)
            {
                return RedirectToAction("blog");

            }
            else
            {
                return RedirectToAction("blog");

            }

        }
        public ActionResult updateCArticle(int CIDupdate, string Cimageupdate, string Ctitleupdate)
        {
            string imagename = "";
            string pathString = "~/images/panelimages";
            if (Cimageupdate != "")
            {


                string oldFileName = Path.Combine(Server.MapPath(pathString), Path.GetFileName(Cimageupdate));
                System.IO.File.Delete(oldFileName);
            }
            if (!Directory.Exists(Server.MapPath(pathString)))
            {
                DirectoryInfo di = Directory.CreateDirectory(Server.MapPath(pathString));
            }



            for (int i = 0; i < Request.Files.Count; i++)
            {

                HttpPostedFileBase hpf = Request.Files[i];

                if (hpf.ContentLength == 0)
                    continue;
                imagename = RandomString(7) + ".jpg";
                string savedFileName = Path.Combine(Server.MapPath(pathString), imagename);
                hpf.SaveAs(savedFileName);



            }
            articleCat model = new articleCat()
            {
                image = imagename,
                title = Ctitleupdate,
                articleCatID = CIDupdate,

            };
            dbRespose response = manager.updataCatArtice(model);
            if (response.status == 200)
            {
                return RedirectToAction("blog");
            }
            else
            {
                return RedirectToAction("blog");
            }

            //string device = RandomString(10);
            //string code = MD5Hash(device + "ncase8934f49909");
            //string result = "";
            //using (WebClient client = new WebClient())
            //{

            //    var collection = new NameValueCollection();
            //    collection.Add("servername", servername);
            //    collection.Add("device", device);
            //    collection.Add("code", code);
            //    collection.Add("image", imagename);
            //    collection.Add("title", Ctitleupdate);
            //    collection.Add("ID", CIDupdate);

            //    byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/UpdateCArticles.php", collection);

            //    result = System.Text.Encoding.UTF8.GetString(response);
            //}


        }

        public void DeleteArticle(string id)
        {
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            //using (WebClient client = new WebClient())
            //{

            //    var collection = new NameValueCollection();
            //    collection.Add("device", device);
            //    collection.Add("code", code);
            //    collection.Add("ID", id);
            //    collection.Add("servername", servername);
            //    byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/DeleteArticles.php", collection);

            //    result = System.Text.Encoding.UTF8.GetString(response);
            //}
            if (true)
            {
                string pathString = "~/images/panelimages";
                string savedFileName = Path.Combine(Server.MapPath(pathString), Path.GetFileName(result));
                System.IO.File.Delete(savedFileName);
            }


            //string imagename = result;
            //string savedFileName = Path.Combine(Server.MapPath(pathString), imagename);
            //System.IO.File.Delete(savedFileName);
        }
        public void DeleteCArticle(string id)
        {
            string device = RandomString(10);
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            //using (WebClient client = new WebClient())
            //{

            //    var collection = new NameValueCollection();
            //    collection.Add("device", device);
            //    collection.Add("code", code);
            //    collection.Add("ID", id);
            //    collection.Add("servername", servername);
            //    byte[] response = client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/DeleteCArticles.php", collection);

            //    result = System.Text.Encoding.UTF8.GetString(response);
            //}
            string pathString = "~/images/panelimages";
            string savedFileName = Path.Combine(Server.MapPath(pathString), Path.GetFileName(result));
            System.IO.File.Delete(savedFileName);

            //string imagename = result;
            //string savedFileName = Path.Combine(Server.MapPath(pathString), imagename);
            //System.IO.File.Delete(savedFileName);
        }
        public PartialViewResult getNewListArticle(string search, string cat)
        {
            List<article> log = getArticleList(cat, search);
            return PartialView("/Views/Shared/admin/_ListOfArticles.cshtml", log);
        }
        [HttpPost]
        public ActionResult GetImageForMCEEditUpload(string filename, HttpPostedFileBase blob)
        {
            string pathString = "~/images/panelimages";
            if (!Directory.Exists(Server.MapPath(pathString)))
            {
                DirectoryInfo di = Directory.CreateDirectory(Server.MapPath(pathString));
            }
            string tobeaddedtoimagename = RandomString(7);
            string savedFileName = Path.Combine(Server.MapPath(pathString), tobeaddedtoimagename + ".jpg");
            blob.SaveAs(savedFileName);
            Session["imageListEdit"] = Session["imageListEdit"] as string + tobeaddedtoimagename + ".jpg,";
            string ss = Session["imageListEdit"] as string;
            // ss = ss + filename;
            ss = ss.Substring(0, ss.Length - 1);
            ViewModel.imageForEMCVM model = new ViewModel.imageForEMCVM();
            return PartialView("/Views/Shared/admin/_imageForMCEEdit.cshtml", ss);
            // return Content(tobeaddedtoimagename);
        }

        [HttpPost]
        public ActionResult GetImageForMCE(string filename, HttpPostedFileBase blob)
        {
            string pathString = "~/images/panelimages";
            if (!Directory.Exists(Server.MapPath(pathString)))
            {
                DirectoryInfo di = Directory.CreateDirectory(Server.MapPath(pathString));
            }
            string tobeaddedtoimagename = RandomString(7);
            string savedFileName = Path.Combine(Server.MapPath(pathString), tobeaddedtoimagename + ".jpg");
            blob.SaveAs(savedFileName);
            Session["imageListAdd"] = Session["imageListAdd"] as string + tobeaddedtoimagename + ".jpg,";
            string ss = Session["imageListAdd"] as string;
            // ss = ss + filename;
            ss = ss.Substring(0, ss.Length - 1);
            ViewModel.imageForEMCVM model = new ViewModel.imageForEMCVM();
            model.data = ss;
            model.type = filename;

            return PartialView("/Views/Shared/admin/_imageForMCE.cshtml", model);
            // return Content(tobeaddedtoimagename);
        }

        public ActionResult DelImageForMCE(string filename, string type, string image)
        {
            string filestring = filename.Replace("../images/panelimages/", "");
            if (type == "edit")
            {

                if (image == "") // اصلی حذف شده 
                {
                    string ss = Session["imageListEdit"] as string;
                    ss = ss.Substring(0, ss.Length - 1);
                    List<string> list = ss.Split(',').ToList();
                    if (list.Count > 1)
                    {
                        list.Remove(filename);
                        string final = "";
                        foreach (var item in list)
                        {
                            final = final + item + ",";
                        }
                        Session["imageListEdit"] = final;

                    }
                    else
                    {
                        return Content("Error");
                    }
                }
                else
                {

                    if (filestring == image)
                    {
                        string ss = Session["imageListEdit"] as string;
                        if (ss == "")
                        {
                            return Content("Error");
                        }
                    }
                }

            }
            string pathString = "~/images/panelimages";
            if (!Directory.Exists(Server.MapPath(pathString)))
            {
                DirectoryInfo di = Directory.CreateDirectory(Server.MapPath(pathString));
            }

            if (filename.Contains("images"))
            {
                string savedFileName = Path.Combine(Server.MapPath(pathString), Path.GetFileName(filename));
                filename = filename.Replace("..", "~");
                System.IO.File.Delete(Server.MapPath(filename));
                return Content("success");
            }
            else
            {
                string savedFileName = Path.Combine(Server.MapPath(pathString), Path.GetFileName(filename));
                System.IO.File.Delete(savedFileName);
                if (type == "edit")
                {
                    string ss = Session["imageListEdit"] as string;
                    ss = ss.Substring(0, ss.Length - 1);
                    List<string> list = ss.Split(',').ToList();
                    list.Remove(filename);
                    string final = "";
                    foreach (var item in list)
                    {
                        final = final + item + ",";
                    }
                    Session["imageListEdit"] = final;
                    if (filestring == image)
                    {
                        return Content("main");
                    }
                    else
                    {
                        return Content("notmain");
                    }

                }
                else
                {
                    string ss = Session["imageListAdd"] as string;
                    ss = ss.Replace(filename + ",", "").Replace(",,", ",");
                    string final = ss;
                    Session["imageListAdd"] = final;

                    //frenlish.ViewModel.imageForEMCVM model = new ViewModel.imageForEMCVM();
                    //model.data = final;
                    //model.type = "add";
                    //return PartialView("/Views/Shared/AdminShared/_imageForMCE.cshtml", model);
                    return Content("");
                }



            }


        }
        public void DelImageForMCEImage(string filename, string type)
        {
            if (type == "aboutus")
            {
                string srt = Request.Cookies["imageAboutUs"].Value;
                srt = srt.Replace(filename + ",", "").Replace(",,", ",");
                Response.Cookies["imageAboutUs"].Value = srt;

            }
            else if (type == "contactus")
            {
                string srt = Request.Cookies["imageContactUs"].Value;
                srt = srt.Replace(filename + ",", "").Replace(",,", ",");
                Response.Cookies["imageContactUs"].Value = srt;
            }
            string pathString = "~/images/panelimages";

            string savedFileName = Path.Combine(Server.MapPath(pathString), Path.GetFileName(filename));
            System.IO.File.Delete(savedFileName);


        }
        //public ActionResult comment()
        //{

        //    string device = RandomString(10);
        //    string code = MD5Hash(device + "ncase8934f49909");
        //    string result = "";
        //    using (WebClient client = new WebClient())
        //    {

        //        var collection = new NameValueCollection();
        //        collection.Add("device", device);
        //        collection.Add("code", code);
        //        collection.Add("servername", servername);
        //        //collection.Add("DayOfWeek", DayOfWeek);
        //        //collection.Add("TimeFrom", TimeFrom);
        //        //collection.Add("TimeTo", TimeTo);
        //        byte[] response =
        //        client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/GetComments.php?", collection);

        //        result = System.Text.Encoding.UTF8.GetString(response);
        //    }

        //    ViewModel.Comments log = JsonConvert.DeserializeObject<frenlish.ViewModel.Comments>(result);
        //    return View(log);
        //}
        //public void setAdminComment(string id, string comment)
        //{
        //    string result = "";
        //    string device = RandomString(10);
        //    string code = MD5Hash(device + "ncase8934f49909");
        //    using (WebClient client = new WebClient())
        //    {

        //        var collection = new NameValueCollection();
        //        collection.Add("servername", servername);
        //        collection.Add("device", device);
        //        collection.Add("code", code);
        //        collection.Add("id", id);
        //        collection.Add("comment", comment);

        //        byte[] response =
        //        client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/setAdminComment.php?", collection);

        //        result = System.Text.Encoding.UTF8.GetString(response);
        //    }
        //}
        //public void delCommnet(string id)
        //{
        //    string result = "";
        //    string device = RandomString(10);
        //    string code = MD5Hash(device + "ncase8934f49909");
        //    using (WebClient client = new WebClient())
        //    {

        //        var collection = new NameValueCollection();
        //        collection.Add("servername", servername);
        //        collection.Add("device", device);
        //        collection.Add("code", code);
        //        collection.Add("id", id);


        //        byte[] response =
        //        client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/delComment.php?", collection);

        //        result = System.Text.Encoding.UTF8.GetString(response);
        //    }
        //}
        //public void changeCommnetActive(string id, string value)
        //{
        //    string result = "";
        //    string device = RandomString(10);
        //    string code = MD5Hash(device + "ncase8934f49909");
        //    using (WebClient client = new WebClient())
        //    {

        //        var collection = new NameValueCollection();
        //        collection.Add("servername", servername);
        //        collection.Add("device", device);
        //        collection.Add("code", code);
        //        collection.Add("id", id);
        //        collection.Add("value", value);

        //        byte[] response =
        //        client.UploadValues(ConfigurationManager.AppSettings["server"] + "/Admin/ChangeCommentStatus.php?", collection);

        //        result = System.Text.Encoding.UTF8.GetString(response);
        //    }
        //}
        #endregion

        #region courseList
        public ActionResult course()
        {
            string query = "";
            var list = manager.getCourseList(0, query).Select(i => new courseVM
            {
                courseID = i.courseID,
                imageTitle = i.imageTitle,
                title = i.title

            }).ToList();
            HomeCourseVM model = new HomeCourseVM()
            {
                coursList = list,
            };
            return View(model);
        }
        public ActionResult addCourse(string title)
        {
            string imageTitle = "";
            string pathString = "~/images/panelimages";
            for (int i = 0; i < Request.Files.Count; i++)
            {

                HttpPostedFileBase hpf = Request.Files[i];

                if (hpf.ContentLength == 0)
                    continue;
                imageTitle = RandomString(7) + ".jpg";
                string savedFileName = Path.Combine(Server.MapPath(pathString), imageTitle);
                hpf.SaveAs(savedFileName);
            }

            dbRespose response = manager.addCourse(title, imageTitle);
            if (response.status == 200)
            {
                return RedirectToAction("course");
            }
            else
            {
                return RedirectToAction("course");

            }
        }
        public ActionResult dellCourse(int CourseID, string imageAddress)
        {
            dbRespose model = manager.deleteCourse(CourseID);
            if (model.status == 200)
            {
                string pathString = "~/images/panelimages";
                string savedFileName = Path.Combine(Server.MapPath(pathString), imageAddress);
                System.IO.File.Delete(savedFileName);
            }
            return RedirectToAction("course");
        }
        public ActionResult editCourse(string titleupdate, string imageupdate, int IDupdate, string formWhere)
        {
            string imageTitle = "";
            string pathString = "~/images/panelimages";
            for (int i = 0; i < Request.Files.Count; i++)
            {

                HttpPostedFileBase hpf = Request.Files[i];

                if (hpf.ContentLength == 0)
                    continue;
                imageTitle = RandomString(7) + ".jpg";
                string savedFileName = Path.Combine(Server.MapPath(pathString), imageTitle);
                hpf.SaveAs(savedFileName);
            }

            dbRespose response = manager.editCourse(titleupdate, imageupdate, imageTitle, IDupdate);
            if (response.status == 200)
            {
                if (imageTitle != "")
                {
                    string savedFileName = Path.Combine(Server.MapPath(pathString), imageupdate);
                    System.IO.File.Delete(savedFileName);
                }

            }
            else
            {

            }
            return RedirectToAction(formWhere);
        }
        public ActionResult ChangeCouserList(string query)
        {
            var list = manager.getCourseList(0, query).Select(i => new courseVM
            {
                courseID = i.courseID,
                imageTitle = i.imageTitle,
                title = i.title

            }).ToList();
            HomeCourseVM model = new HomeCourseVM()
            {
                coursList = list,
            };
            return PartialView("/Views/Shared/admin/_ListOfCourses.cshtml", model);
        }

        #endregion

        #region sessionList
        public ActionResult session(int courseID, string query)
        {

            var list = manager.getsessionList(0, courseID, query).Select(i => new sessionVM
            {
                title = i.title,
                courseID = i.courseID,
                imageTitle = i.imageTitle,
                sessionID = i.sessionID
            }).ToList();
            HomeSessionVM model = new HomeSessionVM()
            {
                sessionList = list,
                courseID = courseID,
                courseTitle = manager.getCourseList(courseID, "").ToList().First().title,
            };
            return View(model);



        }
        public ActionResult addsession(string title, int courseID)
        {
            string imageTitle = "";
            string pathString = "~/images/panelimages";
            for (int i = 0; i < Request.Files.Count; i++)
            {

                HttpPostedFileBase hpf = Request.Files[i];

                if (hpf.ContentLength == 0)
                    continue;
                imageTitle = RandomString(7) + ".jpg";
                string savedFileName = Path.Combine(Server.MapPath(pathString), imageTitle);
                hpf.SaveAs(savedFileName);
            }

            dbRespose response = manager.addsession(title, imageTitle, courseID);
            if (response.status == 200)
            {
                return RedirectToAction("session", new { courseID = courseID });
            }
            else
            {
                return RedirectToAction("session", new { courseID = courseID });
            }
        }
        public ActionResult dellsession(int sessionID, string imageAddress, int courseID)
        {
            dbRespose model = manager.deletesession(sessionID);
            if (model.status == 200)
            {
                string pathString = "~/images/panelimages";
                string savedFileName = Path.Combine(Server.MapPath(pathString), imageAddress);
                System.IO.File.Delete(savedFileName);
            }
            return RedirectToAction("session", new { courseID = courseID });

        }
        public ActionResult editsession(string titleupdate, string imageupdate, int IDupdate, string formWhere)
        {
            string imageTitle = "";
            string pathString = "~/images/panelimages";
            for (int i = 0; i < Request.Files.Count; i++)
            {

                HttpPostedFileBase hpf = Request.Files[i];

                if (hpf.ContentLength == 0)
                    continue;
                imageTitle = RandomString(7) + ".jpg";
                string savedFileName = Path.Combine(Server.MapPath(pathString), imageTitle);
                hpf.SaveAs(savedFileName);
            }

            dbRespose response = manager.editsession(titleupdate, imageupdate, imageTitle, IDupdate);
            if (response.status == 200)
            {
                if (imageTitle != "")
                {
                    string savedFileName = Path.Combine(Server.MapPath(pathString), imageupdate);
                    System.IO.File.Delete(savedFileName);
                }

            }
            else
            {

            }
            if (formWhere == "session")
            {
                return RedirectToAction(formWhere, new { courseID = Int32.Parse(response.data) });
            }
            else
            {
                return RedirectToAction(formWhere);
            }


        }
        public ActionResult ChangeSessionList(int courseID, string query)
        {
            var list = manager.getsessionList(0, courseID, query).Select(i => new sessionVM
            {
                title = i.title,
                courseID = i.courseID,
                imageTitle = i.imageTitle,
                sessionID = i.sessionID
            }).ToList();
            HomeSessionVM model = new HomeSessionVM()
            {
                sessionList = list,
                courseID = courseID,
                courseTitle = manager.getCourseList(courseID, "").ToList().First().title,
            };
            return PartialView("/Views/Shared/admin/_ListOfSessions.cshtml", model);
        }


        #endregion


        #region lessonList
        public ActionResult lesson(int sessionID, string query, string message)
        {
            var list = manager.getlessonList(0, sessionID, query).Select(i => new lessonVM
            {
                title = i.title,
                grammer = i.grammer,
                imageTitle = i.imageTitle,
                lessonID = i.lessonID,
                videoTitle = i.videoTitle

            }).ToList();
            HomeLessonVM model = new HomeLessonVM()
            {
                lessonList = list,
                sessionID = sessionID,
                sessionTitle = manager.getsessionList(sessionID, 0, "").ToList().First().title,
            };
            return View(model);

        }
        public ActionResult addlesson(addLessonVM model)
        {
            string imageTitle = "";
            string pathString = "~/images/panelimages";
            for (int i = 0; i < Request.Files.Count; i++)
            {

                HttpPostedFileBase hpf = Request.Files[i];

                if (hpf.ContentLength == 0)
                    continue;
                imageTitle = RandomString(7) + ".jpg";
                string savedFileName = Path.Combine(Server.MapPath(pathString), imageTitle);
                hpf.SaveAs(savedFileName);
            }

            dbRespose response = manager.addlesson(model.title, imageTitle, model.sessionID, model.grammer, model.videoTitle);
            string message = "";
            if (response.status == 200)
            {
                message = "success";
            }
            else
            {

            }
            return RedirectToAction("lesson", new { sessionID = model.sessionID, message = message });
        }
        public ActionResult delllesson(int lessonID, string imageAddress, string videoAddress)
        {

            dbRespose model = manager.deletelesson(lessonID);
            string message = "";
            if (model.status == 200)
            {
                string pathString = "~/images/panelimages";
                string savedFileName = Path.Combine(Server.MapPath(pathString), imageAddress);
                System.IO.File.Delete(savedFileName);
                string VideosavedFileName = Path.Combine(Server.MapPath(pathString), videoAddress);
                System.IO.File.Delete(VideosavedFileName);
                message = "success";
            }
            else
            {
                message = "fail ";

            }

            return RedirectToAction("lesson", new { sessionID = model.data, message = message });

        }
        public ActionResult editlesson(editLessonVM model)
        {
            string imageTitle = "";
            string pathString = "~/images/panelimages";
            for (int i = 0; i < Request.Files.Count; i++)
            {

                HttpPostedFileBase hpf = Request.Files[i];

                if (hpf.ContentLength == 0)
                    continue;
                imageTitle = RandomString(7) + ".jpg";
                string savedFileName = Path.Combine(Server.MapPath(pathString), imageTitle);
                hpf.SaveAs(savedFileName);
            }

            dbRespose response = manager.editlesson(model.titleupdate, model.oldImageAddress, imageTitle, model.sessionID, model.grammerUpdate, model.oldVideoAddress, model.videoAddressUpdate, model.IDupdate);
            string message = "";
            if (response.status == 200)
            {
                if (imageTitle != "")
                {
                    string savedFileName = Path.Combine(Server.MapPath(pathString), model.oldImageAddress);
                    System.IO.File.Delete(savedFileName);

                }
                if (model.oldVideoAddress != model.videoAddressUpdate && model.videoAddressUpdate != "")
                {
                    string savedFileName = Path.Combine(Server.MapPath(pathString), model.oldVideoAddress);
                    System.IO.File.Delete(savedFileName);

                }
            }
            else
            {

            }
            return RedirectToAction("lesson", new { sessionID = model.sessionID, message = message });

        }
        public ActionResult ChangeLessonList(int sessionID, string query)
        {
            var list = manager.getlessonList(0, sessionID, query).Select(i => new lessonVM
            {
                title = i.title,
                grammer = i.grammer,
                imageTitle = i.imageTitle,
                lessonID = i.lessonID,
                videoTitle = i.videoTitle

            }).ToList();
            HomeLessonVM model = new HomeLessonVM()
            {
                lessonList = list,
                sessionID = sessionID,
                sessionTitle = manager.getsessionList(sessionID, 0, "").ToList().First().title,
            };

            return PartialView("/Views/Shared/admin/_ListOfLessons.cshtml", model);
        }

        #endregion


        #region biLingaulList
        public ActionResult biLingaul(int lessonID, string query)
        {
            var list = manager.getbiLingaulList(0, lessonID, query).Select(i => new bilingaulVM
            {
                bilingaulID = i.bilingaulID,
                Etitle = i.Etitle,
                lessonID = i.lessonID,
                Ptitle = i.Ptitle
            }).ToList();
            HomeBilingualVM model = new HomeBilingualVM()
            {
                bilinaualList = list,
                lessonID = lessonID,
                lessonTitle = manager.getlessonList(lessonID, 0, "").ToList().First().title,
            };
            return View(model);

        }
        public ActionResult addbiLingaul(string Ptitle, string Etitle, int lessonID)
        {


            dbRespose response = manager.addbiLingaul(Ptitle, Etitle, lessonID);
            if (response.status == 200)
            {
                return RedirectToAction("biLingaul", new { lessonID = lessonID });
            }
            else
            {
                return RedirectToAction("biLingaul", new { lessonID = lessonID });

            }
        }
        public ActionResult dellbiLingaul(string biLingaulID)
        {
            dbRespose model = manager.deletebiLingaul(Int32.Parse(biLingaulID));
            return RedirectToAction("biLingaul", new { lessonID = model.data });
        }
        public ActionResult editbiLingaul(string titleupdate, string Etitleupdate, int lessonID, int IDupdate)
        {


            dbRespose response = manager.editbiLingaul(titleupdate, Etitleupdate, IDupdate);
            if (response.status == 200)
            {
                return RedirectToAction("biLingaul", new { lessonID = lessonID });
            }
            else
            {
                return RedirectToAction("biLingaul", new { lessonID = lessonID });
            }
        }
        #endregion


        #region questionList
        public ActionResult question()
        {
            string query = "";
            var model = manager.getquestionList(query);
            return View(model);
        }
        public void addquestion(string Ptitle, string Etitle, int lessonID, int referencID)
        {
            string imageTitle = "";
            string pathString = "~/images/CoursImages";
            for (int i = 0; i < Request.Files.Count; i++)
            {

                HttpPostedFileBase hpf = Request.Files[i];

                if (hpf.ContentLength == 0)
                    continue;
                imageTitle = RandomString(7) + ".jpg";
                string savedFileName = Path.Combine(Server.MapPath(pathString), imageTitle);
                hpf.SaveAs(savedFileName);
            }

            dbRespose response = manager.addquestion(Ptitle, Etitle, lessonID, referencID);
            if (response.status == 200)
            {

            }
            else
            {

            }
        }
        public void dellquestion(int questionID)
        {
            manager.deletequestion(questionID);
        }
        public void editquestion(string Ptitle, string Etitle, int lessonID, int referencID, List<questionOption> questionOptions, int ID)
        {
            string imageTitle = "";
            string pathString = "~/images/CoursImages";
            for (int i = 0; i < Request.Files.Count; i++)
            {

                HttpPostedFileBase hpf = Request.Files[i];

                if (hpf.ContentLength == 0)
                    continue;
                imageTitle = RandomString(7) + ".jpg";
                string savedFileName = Path.Combine(Server.MapPath(pathString), imageTitle);
                hpf.SaveAs(savedFileName);
            }

            dbRespose response = manager.editquestion(Ptitle, Etitle, lessonID, referencID, questionOptions, ID);
            if (response.status == 200)
            {

            }
            else
            {

            }
        }
        #endregion

        #region questionOptionList
        public ActionResult questionOption()
        {
            string query = "";
            var model = manager.getquestionOptionList(query);
            return View(model);
        }
        public void addquestionOption(string title, bool isTrue, int questionID)
        {

            dbRespose response = manager.addquestionOption(title, isTrue, questionID);
            if (response.status == 200)
            {

            }
            else
            {

            }
        }
        public void dellquestionOption(int questionOptionID)
        {
            manager.deletequestionOption(questionOptionID);
        }
        public void editquestionOption(string title, bool isTrue, int questionID, int ID)
        {


            dbRespose response = manager.editquestionOption(title, isTrue, questionID, ID);
            if (response.status == 200)
            {

            }
            else
            {

            }
        }
        #endregion


        #region referencList
        public ActionResult referenc(int lessonID, string qu, string message)
        {
            var list = manager.getreferencList(0, lessonID, qu).Select(i => new referencVM
            {
                contenOrUrl = i.contenOrUrl,
                lessonID = i.lessonID,
                referencID = i.referencID,
                type = i.type,
                title = i.title

            }).ToList();
            HomeRefrenceVM model = new HomeRefrenceVM()
            {
                 
                 ReferenceList = list,
                 lesssonID = lessonID,
                 lessonTitle = manager.getlessonList(lessonID, 0, "").ToList().First().title,
            };
            return View(model);
            
        }
        public void addreferenc(string contenOrUrl, string type, int questionID)
        {

            dbRespose response = manager.addreferenc(contenOrUrl, type, questionID);
            if (response.status == 200)
            {

            }
            else
            {

            }
        }
        public ActionResult dellReference( int referencID , string audioAddress)
        {
            
            dbRespose model =  manager.deletereferenc(referencID);
            string message = "";
            if (model.status == 200)
            {
                if (audioAddress != "")
                {

                    string pathString = "~/images/panelimages";
                    string oldFileName = Path.Combine(Server.MapPath(pathString), Path.GetFileName(audioAddress));
                    System.IO.File.Delete(oldFileName);
                }
                message = "success";
            }
            else
            {
                message = "fail ";

            }

            return RedirectToAction("referenc", new { lessonID = model.data, message = message });
        }
        public void editreferenc(editReferenceVM model)
        {


            dbRespose response = manager.editreferenc( model.titleupdate, model.lessonID, model.typeupdate, model.contextUpdate, model.oldAudioAddress, model.audioAddressUpdate, model.IDupdate);
            if (response.status == 200)
            {
                if (model.oldAudioAddress != model.audioAddressUpdate && model.audioAddressUpdate != "")
                {
                    string pathString = "~/images/panelimages";
                    string savedFileName = Path.Combine(Server.MapPath(pathString), model.oldAudioAddress);
                    System.IO.File.Delete(savedFileName);

                }
            }
            else
            {

            }
        }
        #endregion


    }

}