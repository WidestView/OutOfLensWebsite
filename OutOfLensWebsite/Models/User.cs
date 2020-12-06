using Microsoft.AspNetCore.Mvc;

namespace OutOfLensWebsite.Models
{
	public class User
	{
		[Remote("CheckState", "Home", ErrorMessage = "This is not a valid State!")]
		public string State { get; set; }
	}
}