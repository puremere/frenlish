using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace frenlish.Models
{
    public class Context : DbContext
    {
       
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public Context() : base("frenlish1")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<Context, frenlish.Migrations.Configuration>());

        }
        public DbSet<articleCat> articleCats { get; set; }
        public DbSet<article> articles { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<session> sessions { get; set; }
        public DbSet<lesson> lessons { get; set; }
        public DbSet<question> questions { get; set; }
        public DbSet<questionOption> questionOptions { get; set; }
        public DbSet<referenc> references { get; set; }
        public DbSet<biLingaul> biLingauls { get; set; }
        
    }
    public class articleCat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int articleCatID { get; set; }
        public string title { get; set; }
        public string titleEn { get; set; }
        public string image { get; set; }

        public virtual ICollection<article> Articles { get; set; }

    }
    public class article
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int arcticleID { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public string image { get; set; }
        public string writer { get; set; }
        public string hashtags { get; set; }
        public string link { get; set; }
        public int view { get; set; }
        public DateTime? date { get; set; }

        public int articleCatID { get; set; }
        public virtual articleCat articleCat { get; set; }
    }
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int courseID { get; set; }
        public string title { get; set; }
        public string  imageTitle { get; set; }

        public virtual ICollection<session> sessions { get; set; }

    }
    public class session
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int sessionID { get; set; }
        public string title { get; set; }
        public string imageTitle { get; set; }

        public int courseID { get; set; }
        public virtual Course course { get; set; }

        public virtual ICollection<lesson> lessons { get; set; }

    }
    public class lesson
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int lessonID { get; set; }
        public string title { get; set; }
        public string imageTitle { get; set; }
        public string videoTitle { get; set; }
        public string grammer { get; set; }

        public ICollection<biLingaul> wordsIdioms { get; set; }
        public virtual ICollection<question> questions { get; set; }
        public List<referenc> referencList { get; set; }

        public int sessionID { get; set; }
        public virtual session session { get; set; }

    }

    public class biLingaul
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int bilingaulID { get; set; }
        public string  Ptitle { get; set; }
        public string  Etitle { get; set; }
        public int  lessonID { get; set; }
        public virtual lesson lesson { get; set; }

    }
    public class question
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int questionID { get; set; }
        public string Ptitle { get; set; }
        public string Etitle { get; set; }

        public int referenc { get; set; }
        

        public int lessonID { get; set; }
        public virtual lesson lesson { get; set; }

        public List<questionOption> optionList { get; set; }

    }
    public class referenc
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int referencID { get; set; }
        public string  type { get; set; }
        public string title { get; set; }
        public string  contenOrUrl { get; set; }
        public int lessonID { get; set; }
        public virtual lesson lesson { get; set; }

      
       
    }
    public class questionOption
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int optionID { get; set; }
        public string  title { get; set; }
        public bool  isTrue { get; set; }
        public int questionID { get; set; }
        public virtual question question { get; set; }


    }




}