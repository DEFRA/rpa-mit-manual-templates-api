using System.ComponentModel.DataAnnotations;

namespace Rpa.Mit.Manual.Templates.Api.Api.Models;

public class ItemRequest
{
    public int? Id { get; set; }
    [Required]
    public required string Name { get; set; }
}

