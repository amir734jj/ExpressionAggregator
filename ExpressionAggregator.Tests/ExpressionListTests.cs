using System;
using Xunit;

namespace ExpressionAggregator.Tests
{
    public class ExpressionListTests
    {
        [Fact]
        public void Test__AddNotNull()
        {
            // Arrange
            var list = new ExpressionList<Person>();
            
            // Act
            list.AddIfNotNull(x => x.FirstName == "Test firstname");
            
            // Assert
            Assert.Equal(1, list.Count);
        }
        
        [Fact]
        public void Test__AddNullNullableField()
        {
            // Arrange
            var list = new ExpressionList<Person>();
            
            // Act
            list.AddIfNotNull(x => x.NullableDateOfBirth == null);
            
            // Assert
            Assert.Equal(0, list.Count);
        }
        
        [Fact]
        public void Test__AddNullNullableNestedField()
        {
            // Arrange
            var list = new ExpressionList<Person>();
            int? value = null;
            
            // Act
            list.AddIfNotNull(x => x.Parents.NullableAge == value);
            
            // Assert
            Assert.Equal(0, list.Count);
        }
    }
}