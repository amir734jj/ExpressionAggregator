using System;

namespace ExpressionAggregator.Tests
{
    public class NestedPersonInfo {
        public string MotherName { get; set; }
        public string FatherName { get; set; }
        public bool Status { get; set; }
        public int Age { get; set; }
        public int? NullableAge { get; set; }
    }
    
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public double Height { get; set; }
        public float Worth { get; set; }
        public long Weight { get; set; }
        public DateTime DateOfBirth { get; set; }
        public char Initial { get; set; }
        public NestedPersonInfo Parents { get; set; }
        public int? NullableAge { get; set; }
        public char? NullableInitial { get; set; }
        public DateTime? NullableDateOfBirth { get; set; }
    }
}