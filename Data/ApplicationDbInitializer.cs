using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using timeliste.Models;

namespace timeliste.Data {
	public class ApplicationDbInitializer {
		public static void Initialize(ApplicationDbContext db, UserManager<ApplicationUser> um, RoleManager<IdentityRole> rm) {
			db.Database.EnsureDeleted();
			db.Database.EnsureCreated();

			var adminRole = new IdentityRole("Admin");
			rm.CreateAsync(adminRole).Wait();

			var admin = new ApplicationUser { UserName = "admin@uia.no", Email = "admin@uia.no", AnsattNr = "111", EmailConfirmed = true };
			um.CreateAsync(admin, "Password1.").Wait();
			um.AddToRoleAsync(admin, "Admin").Wait();

			var user = new ApplicationUser { UserName = "user@uia.no", Email = "user@uia.no", AnsattNr = "112", EmailConfirmed = true };
			um.CreateAsync(user, "Password1.").Wait();
			
			var lederRole = new IdentityRole("Leder");
			rm.CreateAsync(lederRole).Wait();
			var leder = new ApplicationUser
			{
				UserName = "leder@uia.no",
				Email = "leder@uia.no",
				AnsattNr = "113",
				EmailConfirmed = true
			};
			um.CreateAsync(leder, "Password1.").Wait();
			um.AddToRoleAsync(leder, "Leder").Wait();

			var regnskap = new ApplicationUser
			{
				UserName = "regnskap@uia.no",
				Email = "regnskap@uia.no",
				AnsattNr = "114",
				EmailConfirmed = true
			};
			var regnskapRole = new IdentityRole("Regnskap");
			rm.CreateAsync(regnskapRole).Wait();
			um.CreateAsync(regnskap, "Password1.").Wait();
			um.AddToRoleAsync(regnskap, "Regnskap").Wait();


			// Adding testing timer.
			var time1 = new Time
			{
				AnsattNr = "112",
				ProsjektId = 1,
				Start = DateTime.Parse("2020-11-10T08:00"),
				Slutt = DateTime.Parse("2020-11-10T16:00"),
				Kommentar = "",
				Timer = (DateTime.Parse("2020-11-10T16:00") - DateTime.Parse("2020-11-10T08:00")).TotalHours
			};

			var time2 = new Time
			{
				AnsattNr = "112",
				ProsjektId = 1,
				Start = DateTime.Parse("2020-11-11T08:00"),
				Slutt = DateTime.Parse("2020-11-11T16:00"),
				Kommentar = "Kommentar",
				Timer = (DateTime.Parse("2020-11-11T16:00") - DateTime.Parse("2020-11-11T08:00")).TotalHours
			};

			var time3 = new Time
			{
				AnsattNr = "112",
				ProsjektId = 2,
				Start = DateTime.Parse("2020-11-10T08:00"),
				Slutt = DateTime.Parse("2020-11-10T16:00"),
				Kommentar = "",
				Timer = (DateTime.Parse("2020-11-10T16:00") - DateTime.Parse("2020-11-10T08:00")).TotalHours
			};

			var time4 = new Time
			{
				AnsattNr = "111",
				ProsjektId = 1,
				Start = DateTime.Parse("2020-11-12T08:00"),
				Slutt = DateTime.Parse("2020-11-12T16:00"),
				Kommentar = "",
				Timer = (DateTime.Parse("2020-11-12T16:00") - DateTime.Parse("2020-11-12T08:00")).TotalHours
			};

			var time5 = new Time
			{
				AnsattNr = "111",
				ProsjektId = 1,
				Start = DateTime.Parse("2020-11-11T08:00"),
				Slutt = DateTime.Parse("2020-11-11T16:00"),
				Kommentar = "",
				Timer = (DateTime.Parse("2020-11-11T16:00") - DateTime.Parse("2020-11-11T08:00")).TotalHours
			};

			db.Time.Add(time1);
			db.Time.Add(time2);
			db.Time.Add(time3);
			db.Time.Add(time4);
			db.Time.Add(time5);

			// Adding testing prosjekt

			var prosjekt1 = new Prosjekt
			{
				ProsjektNavn = "Prosjekt 1",
				KundeNavn = "Kunde Kundesen",
				Info = "Blah blah blah",
				PeriodeLengde = 31
			};

			var prosjekt2 = new Prosjekt
			{
				ProsjektNavn = "Prosjekt 2",
				KundeNavn = "Ola Olasen AS",
				Info = "Da ad da dadadadad",
				PeriodeLengde = 7
			};

			db.Prosjekt.Add(prosjekt1);
			db.Prosjekt.Add(prosjekt2);

			db.SaveChanges();
		}
	}
}
