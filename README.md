# ExpressionAggregator
Linq Expression aggregation, given list of Expressions it creates aggregated expression or Func. This is useful if
you conditionally added expressions to a list of expressions and want to aggregated them all together into one expression or Func.

#### Interface:
```csharp
    public interface IExpressionAggregator<T>
    {
        Func<T, bool> AggregateAndCompile(IEnumerable<Expression<Func<T, bool>>> list);
        
        Expression<Func<T, bool>> Aggregate(IEnumerable<Expression<Func<T, bool>>> list);
    }
```

### Sample code:
```
// accumulator by defualt is "And"
var list = new List<Expression<Func<Person, bool>>>();

list.Add(x => x.Age > 10);
list.Add(x => x.DateOfBirth > DateTime.MinValue);

// Act
var people = _fixture.CreateMany<Person>();
var func =_aggregator.AggregateAndCompile(list);

// Assert
Assert.Equal(people.Count(), people.Where(func).Count());
```
