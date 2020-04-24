using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using frenlish.Models;

namespace frenlish.ViewModel
{
    public class ViewModel
    {
    }
    public class HomeCourseVM
    {
      public  List<courseVM> coursList { get; set; }
    }
    public class courseVM
    {
        public int courseID { get; set; }
        public string title { get; set; }
        public string imageTitle { get; set; }

    }
    public class bilingaulVM
    {
        public int bilingaulID { get; set; }
        public string Ptitle { get; set; }
        public string Etitle { get; set; }
        public int lessonID { get; set; }
    }
    public class HomeSessionVM
    {
        public List<sessionVM> sessionList { get; set; }
        public int courseID { get; set; }
        public string courseTitle { get; set; }
    }
    public class HomeLessonVM
    {
        public List<lessonVM> lessonList { get; set; }
        public int sessionID { get; set; }
        public  string  sessionTitle { get; set; }
    }
    public class HomeBilingualVM
    {
        public List<bilingaulVM> bilinaualList { get; set; }
        public int lessonID { get; set; }
        public string lessonTitle { get; set; }
    }
    public class HomeRefrenceVM
    {
        public List<referencVM> ReferenceList { get; set; }
        public int lesssonID { get; set; }
        public string lessonTitle { get; set; }
    }
    public class sessionVM
    {
        public int sessionID { get; set; }
        public string title { get; set; }
        public string imageTitle { get; set; }

        public int courseID { get; set; }
    }
    public class lessonVM
    {
        public int lessonID { get; set; }
        public string title { get; set; }
        public string imageTitle { get; set; }
        public string videoTitle { get; set; }
        public string grammer { get; set; }
    }
   public class referencVM
    {
        public int referencID { get; set; }
        public string type { get; set; }
        public string title { get; set; }
        public string contenOrUrl { get; set; }
        public int lessonID { get; set; }
    }



    public class AdminBlogVM
    {

        public List<articleVM> Articlelist { get; set; }
        public List<articleCatVM> Catlist { get; set; }
    }
    public class articleVM
    {
        public int arcticleID { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public string image { get; set; }
        public string writer { get; set; }
        public string hashtags { get; set; }
        public string link { get; set; }
        public int view { get; set; }
        public DateTime? date { get; set; }
        public string catname { get; set; }

        public int articleCatID { get; set; }
    }
    public class articleCatVM
    {
        public int articleCatID { get; set; }
        public string title { get; set; }
        public string titleEn { get; set; }
        public string image { get; set; }
    }

    public class imageForEMCVM
    {
        public string data { get; set; }
        public string type { get; set; }
    }
    public class newArcticelVM
    {
        public string image { get; set; }
        public int catList { get; set; }
        public string title { get; set; }
        [AllowHtml]
        public string description { get; set; }
        public string tag { get; set; }
    }
    public class updateArticleVM
    {
        public string IDupdate { get; set; }
        public int catListupdate { get; set; }
        public string titleupdate { get; set; }
        [AllowHtml]
        public string descriptionupdate { get; set; }
        public string tagupdate { get; set; }
    }
    public class dbRespose
    {
        public int status { get; set; }
        public string data { get; set; }
    }

    public class addLessonVM
    {
        public string title { get; set; }
        public int ID { get; set; }
        public string videoTitle { get; set; }
        [AllowHtml]
        public string grammer { get; set; }
        public int sessionID { get; set; }
    }
    public class editLessonVM
    {
        public string titleupdate { get; set; }
        public string videoAddressUpdate { get; set; }
        public string oldVideoAddress { get; set; }
        public string oldImageAddress { get; set; }
        public int sessionID { get; set; }
        [AllowHtml]
        public string grammerUpdate { get; set; }
      
        public int IDupdate { get; set; }
    }

    public class editReferenceVM
    {
        public string titleupdate { get; set; }
        public string audioAddressUpdate { get; set; }
        public string oldAudioAddress { get; set; }
        public string typeupdate { get; set; }
        public int lessonID { get; set; }
        [AllowHtml]
        public string contextUpdate { get; set; }

        public int IDupdate { get; set; }
    }


}