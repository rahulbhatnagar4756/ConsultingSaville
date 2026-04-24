using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace Library.Frog.Models;

public class FrogLookupModel
{
    public string? UUID { get; set; }
    public string? Name { get; set; }
    public string? Result { get; set; }
    public bool? isAnswer { get; set; }
    public bool? isMultiCorrectAnswer { get; set; }
    public bool? isSetOfAnswers { get; set; }
    public string? Color { get; set; }
    public int? OrderVal { get; set; }
    public int? isDeleted { get; set; }
} 
