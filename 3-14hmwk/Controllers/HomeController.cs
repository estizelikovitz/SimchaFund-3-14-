using _3_14hmwk.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimchaFundLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace _3_14hmwk.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=SimchaFund;Integrated Security=true;";

        public IActionResult Index()
        {
            SimchaFundDB db = new(_connectionString);
            SimchosVM vm = new();
            vm.Simchos = db.GetSimchos();
            vm.Contributions = db.GetContributions();
            vm.Contributors = db.GetContributors();
            return View(vm);
        }
        public IActionResult Contributions(int simchaId)
        {
            SimchaFundDB db = new(_connectionString);
            ContributionsVM vm = new();
            vm.Contributions = db.GetContributions().ToList();
            vm.Contributors = db.GetContributors().ToList();
            vm.Deposits = db.GetDeposits().ToList();
            vm.Simchos = db.GetSimchos().ToList();
            vm.Id = simchaId;
            return View(vm);
        }
        public IActionResult Contributors()
        {
            SimchaFundDB db = new(_connectionString);
            ContributorsVM vm = new();
            vm.Contributions = db.GetContributions().ToList();
            vm.Contributors = db.GetContributors().ToList();
            vm.Deposits = db.GetDeposits().ToList();
            return View(vm);
        }
        [HttpPost]
        public IActionResult NewContributor(Contributor contributor, DateTime date, decimal initialDeposit)
        {
            SimchaFundDB db = new(_connectionString);
            Deposit deposit = new();
            deposit.ContributorId = db.AddContributor(contributor);
            deposit.Amount = initialDeposit;
            deposit.Date = date;
            db.AddDeposit(deposit);
            return Redirect("/Home/Contributors");
        }
        public IActionResult History(int contributorId)
        {
            SimchaFundDB db = new(_connectionString);
            List<Event> events = new();
            List<Contribution> contributions = db.GetContributions().Where(c=>c.ContributorId==contributorId).ToList();
            List<Deposit> deposits = db.GetDeposits().Where(d => d.ContributorId == contributorId).ToList();
            List<Simcha> simchos = db.GetSimchos();
            foreach(Contribution c in contributions)
            {
                Event e = new();
                e.Amount = (c.Amount)*-1;
                Simcha s = db.GetSimchos().FirstOrDefault(s => s.Id == c.SimchaId);
                e.Date = s.Date;
                e.Name = $"Contribution for the {s.Name} simcha";
                events.Add(e);
            }
            foreach (Deposit d in deposits)
            {
                Event e = new();
                e.Amount = d.Amount;
                e.Date = d.Date;
                e.Name = "Deposit";
                events.Add(e);
            }
            EventsVM vm = new();
            vm.Events = events.OrderBy(e => e.Date).ToList();
            vm.Name = $"{db.GetContributors().FirstOrDefault(c => c.Id == contributorId).FirstName} {db.GetContributors().FirstOrDefault(c => c.Id == contributorId).LastName}";
            return View(vm);
        }

        [HttpPost]
        public IActionResult NewSimcha(Simcha simcha)
        {
            SimchaFundDB db = new(_connectionString);
            db.AddSimcha(simcha);
            return Redirect("/Home/Index");
        }
        [HttpPost]
        public IActionResult NewDeposit(Deposit deposit)
        {
            SimchaFundDB db = new(_connectionString);
            db.AddDeposit(deposit);
            return Redirect("/Home/Contributors");
        }
        [HttpPost]
        public IActionResult EditContributor(Contributor contributor)
        {
            SimchaFundDB db = new(_connectionString);
            db.EditContributor(contributor);
            return Redirect("/Home/Contributors");
        }
        [HttpPost]
        public IActionResult UpdateContributions(List<ContributionWithInclude> contributionswi, int simchaId)
        {
            foreach(ContributionWithInclude cwi in contributionswi)
            {
                bool x=cwi.Include;
            }
            SimchaFundDB db = new(_connectionString);
            db.Delete(simchaId);

            foreach (ContributionWithInclude cw in contributionswi)
            {
                if (cw.Include)
                {
                    Contribution c= new();
                    c.ContributorId = cw.ContributorId;
                    c.SimchaId = simchaId;
                    c.Amount = cw.Amount;
                    db.AddContribution(c);
                }
            }
            return Redirect("/Home/Index");
        }
    }
}

