using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace OutOfLens_ASP.Models
{
	public class User
	{
		[Remote("CheckState", "Home", ErrorMessage = "This is not a valid State!")]
		public string State { get; set; }
	}
}