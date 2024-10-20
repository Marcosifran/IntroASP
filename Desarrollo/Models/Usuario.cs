using System;
using System.Collections.Generic;

namespace Desarrollo.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Contraseña { get; set; } = null!;

    public int? Equipoid { get; set; }

    public virtual Equipo? Equipo { get; set; }
}
