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

// add expressions
list.Add(x => x.Age > 10);
list.Add(x => x.DateOfBirth > DateTime.MinValue);

// create sample size
var people = _fixture.CreateMany<Person>();

// aggregate expressions into one large expression and compile expression to Func
var func =_aggregator.AggregateAndCompile(list);

// use the Func is Linq Where clause
var filteredPeople = people.Where(func).Count());
```
