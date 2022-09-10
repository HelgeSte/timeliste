﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace timeliste.Models {
	public class ApplicationUser : IdentityUser {
		public string AnsattNr { get; set; }

		public string FirstName { get; set; }
		public string LastName { get; set; }
	}
}
