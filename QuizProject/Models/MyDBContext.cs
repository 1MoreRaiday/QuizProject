using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizProject.Models
{
    internal class MyDBContext : DbContext
    {
        public MyDBContext() {
            if (this.Database.EnsureCreated())
            {
                this.Questions.Add(new QuestionModel { Title = "Акулы", Text = "Правда ли, что акулам надо продолжать двигаться чтобы плыть?", Answer = true });
                this.Questions.Add(new QuestionModel { Title = "Кошки", Text = "Могут ли кошки видеть в полной темноте?", Answer = true });
                this.Questions.Add(new QuestionModel { Title = "Птицы", Text = "Могут ли птицы спать в полете?", Answer = true });
                this.Questions.Add(new QuestionModel { Title = "Рыбы", Text = "Могут ли рыбы чувствовать боль?", Answer = false });
                this.Questions.Add(new QuestionModel { Title = "ИИ", Text = "Правда ли, что эти вопросы сгенерированы нейросетью в нужном формате за один промт?", Answer = true });
                this.Questions.Add(new QuestionModel { Title = "ИИ", Text = "У программистов останется работа?", Answer = false });
            }

        }



        public DbSet<AccountModel> Accounts { get; set; }
        public DbSet<QuestionModel> Questions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=" + Path.GetTempPath() + "\\Quiz.db");
            base.OnConfiguring(optionsBuilder);
        }

    }
}
