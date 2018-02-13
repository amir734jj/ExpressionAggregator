using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoFixture;
using Xunit;

namespace ExpressionAggregator.Tests
{
    public class ExpressionAggregatorTests : IDisposable
    {
        private List<Expression<Func<Person, bool>>> _list;
        private Fixture _fixture;
        private ExpressionAggregator<Person> _aggregator;
        private const int SampleSize = 100;

        public ExpressionAggregatorTests()
        {
            _aggregator = new ExpressionAggregator<Person>();
            _list = new List<Expression<Func<Person, bool>>>();
            _fixture = new Fixture();
        }
        
        [Fact]
        public void Test__AggregateTrueALlBasic()
        {
            // Arrange
            _fixture.Customize<Person>(x => x.With(p => p.Age, 20)
                .With(p => p.DateOfBirth, DateTime.Now)
            );
            
            _list.Add(x => x.Age > 10);
            _list.Add(x => x.DateOfBirth > DateTime.MinValue);

            // Act
            var people = _fixture.CreateMany<Person>(SampleSize).ToList();
            var func =_aggregator.AggregateAndCompile(_list);

            // Assert
            Assert.Equal(people.Count(), people.Where(func).Count());
        }
        
        [Fact]
        public void Test__AggregateTrueSomeBasic()
        {
            // Arrange
            _fixture.Customize<Person>(x => x.With(p => p.DateOfBirth, DateTime.Now));
            
            _list.Add(x => x.Age % 2 == 0);
            _list.Add(x => x.DateOfBirth > DateTime.MinValue);

            // Act
            var people = _fixture.CreateMany<Person>(SampleSize).ToList();
            var func =_aggregator.AggregateAndCompile(_list);

            // Assert
            Assert.NotEqual(people.Count(), people.Where(func).Count());
        }
        
        [Fact]
        public void Test__AggregateAllComplex()
        {
            // Arrange
            _fixture.Customize<Person>(x => x.With(p => p.Age, 20)
                .With(p => p.DateOfBirth, DateTime.Now)
                .With(p => p.Parents, new NestedPersonInfo()
                {
                    FatherName = "Some firstname"
                })
            );            
            
            _list.Add(x => x.Age == 20);
            _list.Add(x => x.DateOfBirth > DateTime.MinValue);
            _list.Add(x => x.Parents.FatherName == "Some firstname");

            // Act
            var people = _fixture.CreateMany<Person>(SampleSize).ToList();
            var func =_aggregator.AggregateAndCompile(_list);

            // Assert
            Assert.Equal(people.Count(), people.Where(func).Count());
        }

        [Fact]
        public void Test__AggregateSomeComplex()
        {
            // Arrange
            _fixture.Customize<Person>(x => x.With(p => p.Age, 20)
                .With(p => p.DateOfBirth, DateTime.Now)
            );            
            
            _list.Add(x => x.Age == 20);
            _list.Add(x => x.Parents.Age % 2 == 0);
            _list.Add(x => x.DateOfBirth > DateTime.MinValue);

            // Act
            var people = _fixture.CreateMany<Person>(SampleSize).ToList();
            var func =_aggregator.AggregateAndCompile(_list);

            // Assert
            Assert.NotEqual(people.Count(), people.Where(func).Count());
        }
        
        [Fact]
        public void Test__AggregateSomeComplexNullable()
        {
            // Arrange
            _fixture.Customize<Person>(x => x.With(p => p.Age, 20)
                .With(p => p.DateOfBirth, DateTime.Now)
                .With(p => p.NullableAge, null)
            );            
            
            _list.Add(x => x.Age == 20);
            _list.Add(x => x.Parents.Age % 2 == 0);
            _list.Add(x => x.DateOfBirth > DateTime.MinValue);
            _list.Add(x => x.NullableAge == null);

            // Act
            var people = _fixture.CreateMany<Person>(SampleSize).ToList();
            var func =_aggregator.AggregateAndCompile(_list);

            // Assert
            Assert.NotEqual(people.Count(), people.Where(func).Count());
        }
        
        [Fact]
        public void Test__AggregateSomeComplexCustomAccumulator()
        {
            // Arrange
            _aggregator = new ExpressionAggregator<Person>(Expression.Or);
            _fixture.Customize<Person>(x => x.With(p => p.Age, 20)
                .With(p => p.DateOfBirth, DateTime.Now)
                .With(p => p.NullableAge, null)
            );            
            
            _list.Add(x => x.Age == 20);
            _list.Add(x => x.Parents.Age % 2 == 0);
            _list.Add(x => x.DateOfBirth > DateTime.MinValue);
            _list.Add(x => x.NullableAge == null);

            // Act
            var people = _fixture.CreateMany<Person>(SampleSize).ToList();
            var func =_aggregator.AggregateAndCompile(_list);

            // Assert
            Assert.Equal(people.Count(), people.Where(func).Count());
        }
        
        public void Dispose()
        {
            _aggregator = new ExpressionAggregator<Person>();
            _fixture = new Fixture();
            _list = new List<Expression<Func<Person, bool>>>();
        }
    }
}