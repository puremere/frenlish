using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using frenlish.Models;
using frenlish.ViewModel;


namespace frenlish.Models
{

    public class dbManger
    {
        Context context = new Context();

        #region articleSection
        public dbRespose addArticle(article model)
        {
            dbRespose response = new dbRespose();
            context.articles.Add(model);
            context.SaveChanges();
            response.status = 200;
            return response;
        }
        public dbRespose updataArtice(article model)
        {
            dbRespose response = new dbRespose();
            try
            {

                article selectedArticle = getArticle(model.arcticleID);
                selectedArticle.hashtags = model.hashtags;
                selectedArticle.image = model.image;
                selectedArticle.title = model.title;
                selectedArticle.content = model.content;
                context.SaveChanges();
                response.status = 200;

            }
            catch (Exception)
            {
                response.status = 400;
            }

            return response;
        }
        public dbRespose addArticleCat(articleCat model)
        {
            dbRespose MR = new dbRespose();
            try
            {
                context.articleCats.Add(model);
                context.SaveChanges();
                MR.status = 200;


            }
            catch (Exception err)
            {

                MR.status = 400;
            }
            return MR;


        }
        public dbRespose updataCatArtice(articleCat model)
        {
            dbRespose response = new dbRespose();

            try
            {

                articleCat selectedArticle = getCatArtice(model.articleCatID);
                if (model.image != "")
                    selectedArticle.image = model.image;
                selectedArticle.title = model.title;
                context.SaveChanges();
                response.status = 200;

            }
            catch (Exception)
            {
                response.status = 400;
            }

            return response;
        }
        public articleCat getCatArtice(int id)
        {
            return context.articleCats.Where(x => x.articleCatID == id).FirstOrDefault();
        }
        public IQueryable<articleCat> getCatArticleList()
        {
            return context.articleCats.AsQueryable();
        }
        public article getArticle(int id)
        {
            return context.articles.Where(x => x.arcticleID == id).FirstOrDefault();
        }
        public IQueryable<article> getArticleList()
        {
            return context.articles.AsQueryable();
        }
        #endregion

        #region course
        public IQueryable<Course> getCourseList(int courseID, string query)
        {
            var model = context.Courses.AsQueryable();
            if (query != "")
            {
                model = model.Where(x => x.title.Contains(query)).AsQueryable();
               
            }
            if (courseID > 0)
            {
                model = model.Where(x => x.courseID == courseID).AsQueryable();

            }

            return model;
        }
        public dbRespose addCourse(string title, string imageTile)
        {
            dbRespose model = new dbRespose();
            try
            {
                context.Courses.Add(new Course
                {
                    imageTitle = imageTile,
                    title = title
                });
                context.SaveChanges();
                model.status = 200;
            }
            catch (Exception)
            {

                model.status = 500;
            }
            return model;

        }
        public dbRespose deleteCourse(int courseID)
        {
            dbRespose model = new dbRespose();
            try
            {
                if (context.sessions.Any(x => x.courseID == courseID))
                {
                    model.status = 500;
                    model.data = "این مجموعه شامل سیلابس درسی می باشد";
                }
                else
                {
                    context.Courses.Remove(context.Courses.SingleOrDefault(x => x.courseID == courseID));
                    model.status = 200;
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {

            }
            return model;
        }
        public dbRespose editCourse(string title,string oldimage, string imageTile, int ID)
        {
            dbRespose model = new dbRespose();
            try
            {
                Course course = context.Courses.SingleOrDefault(x => x.courseID == ID);
                course.title = title;
                if (oldimage != imageTile && imageTile != "")
                {
                    course.imageTitle = imageTile;
                }
                context.SaveChanges();
                model.status = 200;
            }
            catch (Exception)
            {
                model.data = "خطا ! لطفاً مجددا تلاش کنید";
                model.status = 500;
            }
            return model;

        }
        #endregion


        #region session
        public IQueryable<session> getsessionList(int sessionID, int courseID, string query)
        {
            var model = context.sessions.AsQueryable();
            if (courseID >0)
            {
                model = model.Where(x => x.courseID == courseID).AsQueryable();
               
            }
            if (query != null)
            {
                model = model.Where(x => x.title.Contains(query)).AsQueryable();
            }
            if (sessionID > 0)
            {
                model = model.Where(x => x.sessionID == sessionID).AsQueryable();
            }
            List<session> lst = model.ToList();
            return model;
        }
        public dbRespose addsession(string title, string imageTile, int courseID)
        {
            dbRespose model = new dbRespose();
            try
            {
                session newSession = new session()
                {
                    imageTitle = imageTile,
                    title = title,
                    courseID = courseID,
                };
                context.sessions.Add(newSession);
                context.Courses.SingleOrDefault(x => x.courseID == courseID).sessions.Add(newSession);
                context.SaveChanges();
                model.status = 200;
            }
            catch (Exception)
            {

                model.status = 500;
            }
            return model;

        }
        public dbRespose deletesession(int sessionID)
        {
            dbRespose model = new dbRespose();
            try
            {
                if (context.lessons.Any(x => x.sessionID == sessionID))
                {
                    model.status = 500;
                    model.data = "خطا! این مجموعه شامل سیلابس درسی می باشد";
                }
                else
                {
                    model.status = 200;
                    context.sessions.Remove(context.sessions.SingleOrDefault(x => x.sessionID == sessionID));
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {

            }
            return model;
        }
        public dbRespose editsession(string title, string oldImage, string imageTile, int ID)
        {
            dbRespose model = new dbRespose();
            try
            {
                session session = context.sessions.SingleOrDefault(x => x.sessionID == ID);
                session.title = title;
                if (imageTile != "" && oldImage != imageTile)
                {
                    session.imageTitle = imageTile;
                }
                context.SaveChanges();
                model.status = 200;
                model.data = session.courseID.ToString();
            }
            catch (Exception)
            {
                model.data = "خطا ! لطفاً مجددا تلاش کنید";
                model.status = 500;
            }
            return model;

        }
        #endregion

        #region lesson
        public IQueryable<lesson> getlessonList(int lessonID, int sessionID, string query)
        {
            var model = context.lessons.AsQueryable();
            if (lessonID != 0)
            {
                model = model.Where(x => x.lessonID == lessonID).AsQueryable();
            }
            if (sessionID != 0)
            {
                model = model.Where(x => x.sessionID == sessionID).AsQueryable();
            }
            if (query != null)
            {
                model = model.Where(x => x.title.Contains(query)).AsQueryable();
            }
            return model;
        }
        public dbRespose addlesson(string title, string imageTile, int sessionID, string grammer, string videoTitle)
        {
            dbRespose model = new dbRespose();
            List<question> questionList = new List<question>();
            List<biLingaul> wordsIdioms = new List<biLingaul>();
            try
            {

                context.lessons.Add(new lesson
                {
                    imageTitle = imageTile,
                    title = title,
                    sessionID = sessionID,
                    grammer = grammer,
                    videoTitle = videoTitle,
                    questions = questionList,
                    wordsIdioms = wordsIdioms,
                });
                context.SaveChanges();
                model.status = 200;
            }
            catch (Exception)
            {

                model.status = 500;
            }
            return model;

        }
        public dbRespose deletelesson(int lessonID)
        {
            dbRespose model = new dbRespose();
            try
            {
                var less = context.lessons.SingleOrDefault(x => x.lessonID == lessonID);
                if (context.biLingauls.Any(x => x.lessonID == lessonID) || context.questions.Any(x => x.lessonID == lessonID) || context.references.Any(x => x.lessonID == lessonID))
                {
                    model.status = 500;
                    model.data = less.sessionID.ToString();
                }
                else
                {
                   
                    context.lessons.Remove(less);
                    context.SaveChanges();
                    model.status = 200;
                    model.data = less.sessionID.ToString();
                }
            }
            catch (Exception)
            {

            }
            return model;
        }
        public dbRespose editlesson(string title, string oldImage, string imageTitle, int sessionID, string grammer, string videotitle,string videoTitleUpdate, int ID)
        {
            dbRespose model = new dbRespose();
            try
            {
                lesson lesson = context.lessons.SingleOrDefault(x => x.lessonID == ID);
                if (title != "")
                    lesson.title = title;

                if (imageTitle != "" && oldImage != imageTitle)
                {
                    lesson.imageTitle = imageTitle;
                };
                if (grammer != "")
                    lesson.grammer = grammer;
                if (videoTitleUpdate != "" && videotitle != videoTitleUpdate)
                    lesson.videoTitle = videoTitleUpdate;
               
                if(sessionID != 0)
                    lesson.sessionID = sessionID;



                context.SaveChanges();
                model.status = 200;
            }
            catch (Exception)
            {
                model.data = "خطا ! لطفاً مجددا تلاش کنید";
                model.status = 500;
            }
            return model;

        }
        #endregion


        #region biLingaul
        public IQueryable<biLingaul> getbiLingaulList(int bilingaulID,int lessonID ,string query)
        {
            var model = context.biLingauls.AsQueryable();
            if (bilingaulID > 0)
            {
                model = model.Where(x => x.bilingaulID == bilingaulID).AsQueryable();

            }
            if (query != null)
            {
                model = model.Where(x => x.Etitle.Contains(query) || x.Ptitle.Contains(query)).AsQueryable();
            }
            if (lessonID > 0)
            {
                model = model.Where(x => x.lessonID == lessonID).AsQueryable();
            }
            List<biLingaul> lst = model.ToList();
            return model;
            
           
        }
        public dbRespose addbiLingaul(string Ptitle, string Etitle, int lessonID)
        {
            dbRespose model = new dbRespose();
          
            try
            {

                context.biLingauls.Add(new biLingaul
                {
                    lessonID = lessonID,
                    Ptitle = Ptitle,
                    Etitle = Etitle,
                   

                });
                context.SaveChanges();
                model.status = 200;
            }
            catch (Exception)
            {

                model.status = 500;
            }
            return model;

        }
        public dbRespose deletebiLingaul(int biLingaulID)
        {
            

            dbRespose model = new dbRespose();
            try
            {
                var less = context.biLingauls.SingleOrDefault(x => x.bilingaulID == biLingaulID);
                if (less == null)
                {
                    model.status = 500;
                    model.data = less.lessonID.ToString();
                }
                else
                {
                    context. biLingauls.Remove(less);
                    context.SaveChanges();
                    model.status = 200;
                    model.data = less.lessonID.ToString();
                }
            }
            catch (Exception)
            {

            }
            return model;
        }
        public dbRespose editbiLingaul(string Ptitle, string Etitle, int ID)
        {
            dbRespose model = new dbRespose();
            try
            {
                biLingaul biLingaul = context.biLingauls.SingleOrDefault(x => x.bilingaulID == ID);
                if (Ptitle != "")
                    biLingaul.Ptitle = Ptitle;
                if (Etitle != "")
                    biLingaul.Etitle = Etitle;
               
               
                context.SaveChanges();
                model.status = 200;
            }
            catch (Exception)
            {
                model.data = "خطا ! لطفاً مجددا تلاش کنید";
                model.status = 500;
            }
            return model;

        }
        #endregion


        #region question
        public IQueryable<question> getquestionList(string query)
        {
            var model = context.questions.AsQueryable();
            return model;
        }
        public dbRespose addquestion(string Ptitle, string Etitle, int lessonID, int referencID)
        {
            dbRespose model = new dbRespose();
            List<questionOption> optionList = new List<questionOption>();
            try
            {

                context.questions.Add(new question
                {
                    lessonID = lessonID,
                    Ptitle = Ptitle,
                    referenc = referencID,
                    Etitle = Etitle,
                    optionList = optionList

                });
                context.SaveChanges();
                model.status = 200;
            }
            catch (Exception)
            {

                model.status = 500;
            }
            return model;

        }
        public dbRespose deletequestion(int questionID)
        {
            dbRespose model = new dbRespose();
            try
            {
                if (context.questions.Any(x => x.questionID == questionID))
                {
                    model.status = 500;
                    model.data = "خطا! این مجموعه شامل سیلابس درسی می باشد";
                }
                else
                {
                    model.status = 200;
                }
            }
            catch (Exception)
            {

            }
            return model;
        }
        public dbRespose editquestion(string Ptitle, string Etitle, int lessonID, int referencID, List<questionOption> questionOptions, int ID)
        {
            dbRespose model = new dbRespose();
            try
            {
                question question = context.questions.SingleOrDefault(x => x.questionID == ID);
                if (Ptitle != "")
                    question.Ptitle = Ptitle;
                if (Etitle != "")
                    question.Etitle = Etitle;
                if (lessonID != 0)
                    question.lessonID = lessonID;
                if (referencID != 0)
                    question.referenc = referencID;
                if (questionOptions != null)
                    question.optionList = questionOptions;
                    context.SaveChanges();
                model.status = 200;
            }
            catch (Exception)
            {
                model.data = "خطا ! لطفاً مجددا تلاش کنید";
                model.status = 500;
            }
            return model;

        }
        #endregion


        #region questionOption
        public IQueryable<questionOption> getquestionOptionList(string query)
        {
            var model = context.questionOptions.AsQueryable();
            return model;
        }
        public dbRespose addquestionOption(string title, bool isTrue, int questionID)
        {
            dbRespose model = new dbRespose();
           
            try
            {

                context.questionOptions.Add(new questionOption
                {
                    title = title,
                     isTrue = isTrue,
                      questionID = questionID

                });
                context.SaveChanges();
                model.status = 200;
            }
            catch (Exception)
            {

                model.status = 500;
            }
            return model;

        }
        public dbRespose deletequestionOption(int questionOptionID)
        {
            dbRespose model = new dbRespose();
            try
            {
                if (context.questionOptions.Any(x => x.optionID == questionOptionID))
                {
                    model.status = 500;
                    model.data = "خطا! این مجموعه شامل سیلابس درسی می باشد";
                }
                else
                {
                    model.status = 200;
                }
            }
            catch (Exception)
            {

            }
            return model;
        }
        public dbRespose editquestionOption(string title, bool isTrue, int questionID,  int ID)
        {
            dbRespose model = new dbRespose();
            try
            {
                questionOption questionOption = context.questionOptions.SingleOrDefault(x => x.optionID == ID);
                if (title != "")
                    questionOption.title = title;
               
                if (questionID != 0)
                    questionOption.questionID  = questionID;
                

                questionOption.isTrue = isTrue;
                context.SaveChanges();
                model.status = 200;
            }
            catch (Exception)
            {
                model.data = "خطا ! لطفاً مجددا تلاش کنید";
                model.status = 500;
            }
            return model;

        }
        #endregion

        #region referenc
        public IQueryable<referenc> getreferencList(int referencID, int lessonID ,string query)
        {
            var model = context.references.AsQueryable();
            if (referencID != 0)
            {
                model = model.Where(x => x.referencID == referencID).AsQueryable();
            }
            if (lessonID != 0)
            {
                model = model.Where(x => x.lessonID == lessonID).AsQueryable();
            }
            if (query != null)
            {
                model = model.Where(x => x.title.Contains(query)).AsQueryable();
            }
            return model;
           
        }
        public dbRespose addreferenc(string contenOrUrl, string type, int questionID)
        {
            dbRespose model = new dbRespose();
          
            try
            {

                context.references.Add(new referenc
                {
                    contenOrUrl = contenOrUrl,
                    //questionID = questionID,
                    type = type,
                       
                       

                });
                context.SaveChanges();
                model.status = 200;
            }
            catch (Exception)
            {

                model.status = 500;
            }
            return model;

        }
        public dbRespose deletereferenc(int referencID)
        {
            dbRespose model = new dbRespose();
            var reference = context.references.SingleOrDefault(x => x.referencID == referencID);
            try
            {
                context.references.Remove(reference);
                context.SaveChanges();
                model.status = 200;
                model.data = reference.lessonID.ToString();
            }
            catch (Exception)
            {

            }
            return model;
        }
        public dbRespose editreferenc(string title, int lessonID,string type, string content, string audiotitle, string audioTitleUpdate, int ID)
        {
            dbRespose model = new dbRespose();
            try
            {
                referenc referenc = context.references.SingleOrDefault(x => x.referencID == ID);
                referenc.type = type;
                if (type == "sot")
                {
                    if (audioTitleUpdate != "" && audioTitleUpdate != audiotitle)
                    {
                        referenc.contenOrUrl = audioTitleUpdate;
                    }
                }
                else
                {
                    referenc.contenOrUrl = content;
                }
                  
                if (title != "")
                {
                    referenc.title = title;
                }
                context.SaveChanges();
                model.status = 200;
            }
            catch (Exception)
            {
                model.data = "خطا ! لطفاً مجددا تلاش کنید";
                model.status = 500;
            }
            return model;

        }
        #endregion
    }
}