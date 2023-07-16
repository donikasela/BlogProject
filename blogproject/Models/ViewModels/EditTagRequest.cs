using System;
namespace blogproject.Models.ViewModels
{
	public class EditTagRequest
	{
        public Guid id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }
    }
}

