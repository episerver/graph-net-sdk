using EpiServer.ContentGraph.UnitTests.QueryTypeObjects;
using EPiServer.ContentGraph.Api;
using EPiServer.ContentGraph.Api.Autocomplete;
using EPiServer.ContentGraph.Api.Facets;
using EPiServer.ContentGraph.Api.Filters;
using EPiServer.ContentGraph.Api.Querying;
using Xunit;

namespace EpiServer.ContentGraph.UnitTests
{
    public class GenerateFilterTests
    {
        readonly TypeQueryBuilder<RequestTypeObject> typeQueryBuilder;
        readonly static string inOpt = "in: [\"1\",\"2\",\"3\"]";
        readonly static string notInOpt = "notIn: [\"4\",\"5\"]";
        readonly static string expectedStringOperator = 
            $"boost: 10,exist: true,startsWith: \"start\",endsWith: \"end\",synonyms: [ONE]," +
            $"contains: \"test\",{inOpt},{notInOpt},like: \"alloy\",eq: \"good\",notEq: \"bad\"";
        StringFilterOperators stringFilterOperators = new StringFilterOperators()
                .Boost(10)
                .Exists(true)
                .StartWith("start")
                .EndWith("end")
                .Synonym(Synonyms.ONE)
                .Contains("test")
                .In("1", "2", "3")
                .NotIn("4", "5")
                .Like("alloy")
                .Eq("good")
                .NotEq("bad");
        public GenerateFilterTests()
        {
            typeQueryBuilder = new TypeQueryBuilder<RequestTypeObject>();
        }
        [Fact]
        public void SingleFieldWithSimpleFiltersTest()
        {
            string expectedFilters = @"(where:{Property1:{boost: 10,exist: true,startsWith: ""start"",endsWith: ""end"",synonyms: [ONE]," +
                @"contains: ""test"",in: [""1"",""2"",""3""],notIn: [""4"",""5""],like: ""alloy"",eq: ""good"",notEq: ""bad""}})";
            string items = "{items{Property1}}";
            string type = "RequestTypeObject";
            typeQueryBuilder.Field(x => x.Property1);
            typeQueryBuilder.Where(x => x.Property1, stringFilterOperators);
            string? query = typeQueryBuilder.ToQuery().GetQuery().Query;

            Assert.Equal(stringFilterOperators.Query, expectedStringOperator);
            Assert.NotNull(query);
            Assert.Contains(expectedFilters, query);
            Assert.Equal($"{type}{expectedFilters}{items}", query);
        }

        [Fact]
        public void SingleFieldWithAndFiltersTest()
        {
            string expectedFilters = @"(where:{_and:[{Property1:{boost: 10,exist: true,startsWith: ""start"",endsWith: ""end"",synonyms: [ONE]," +
                @"contains: ""test"",in: [""1"",""2"",""3""],notIn: [""4"",""5""],like: ""alloy"",eq: ""good"",notEq: ""bad""}},{Property3:{NestedProperty:{eq: 1}}}]})";
            string items = "{items{Property1}}";
            string type = "RequestTypeObject";

            IFilter andFilter = new AndFilter<RequestTypeObject>(x => x.Property1, stringFilterOperators)
                .And(x=>x.Property3.NestedProperty, new NumericFilterOperators().Eq(1));
            typeQueryBuilder.Field(x => x.Property1);
            typeQueryBuilder.Where(andFilter);
            string? query = typeQueryBuilder.ToQuery().GetQuery().Query;
            Assert.Equal(stringFilterOperators.Query, expectedStringOperator);
            Assert.NotNull(query);
            Assert.Contains(expectedFilters, query);
            Assert.Equal($"{type}{expectedFilters}{items}", query);
        }
        [Fact]
        public void SingleFieldWithOrFiltersTest()
        {
            string expectedFilters = @"(where:{_or:[{Property1:{boost: 10,exist: true,startsWith: ""start"",endsWith: ""end"",synonyms: [ONE]," +
                @"contains: ""test"",in: [""1"",""2"",""3""],notIn: [""4"",""5""],like: ""alloy"",eq: ""good"",notEq: ""bad""}},{Property3:{NestedProperty:{eq: 1}}}]})";
            string items = "{items{Property1}}";
            string type = "RequestTypeObject";

            IFilter andFilter = new OrFilter<RequestTypeObject>(x => x.Property1, stringFilterOperators)
                .Or(x => x.Property3.NestedProperty, new NumericFilterOperators().Eq(1));
            typeQueryBuilder.Field(x => x.Property1);
            typeQueryBuilder.Where(andFilter);
            string? query = typeQueryBuilder.ToQuery().GetQuery().Query;
            Assert.Equal(stringFilterOperators.Query, expectedStringOperator);
            Assert.NotNull(query);
            Assert.Contains(expectedFilters, query);
            Assert.Equal($"{type}{expectedFilters}{items}", query);
        }
        [Fact]
        public void SingleFieldWithNotFiltersTest()
        {
            string expectedFilters = @"(where:{_not:[{Property1:{boost: 10,exist: true,startsWith: ""start"",endsWith: ""end"",synonyms: [ONE]," +
                @"contains: ""test"",in: [""1"",""2"",""3""],notIn: [""4"",""5""],like: ""alloy"",eq: ""good"",notEq: ""bad""}},{Property3:{NestedProperty:{eq: 1}}}]})";
            string items = "{items{Property1}}";
            string type = "RequestTypeObject";

            IFilter andFilter = new NotFilter<RequestTypeObject>(x => x.Property1, stringFilterOperators)
                .Not(x => x.Property3.NestedProperty, new NumericFilterOperators().Eq(1));
            typeQueryBuilder.Field(x => x.Property1);
            typeQueryBuilder.Where(andFilter);
            string? query = typeQueryBuilder.ToQuery().GetQuery().Query;
            Assert.Equal(stringFilterOperators.Query, expectedStringOperator);
            Assert.NotNull(query);
            Assert.Contains(expectedFilters, query);
            Assert.Equal($"{type}{expectedFilters}{items}", query);
        }
        [Fact]
        public void generate_where_with_raw_string()
        {
            const string expectedFields = "{items{Property1}}";
            const string expectedFilter = "where:{Nesteds:{NestedProperty:{eq: 100}}}";
            const string expectedFullQuery = $"RequestTypeObject({expectedFilter}){expectedFields}";

            typeQueryBuilder.Field(x => x.Property1);
            typeQueryBuilder.Where("Nesteds.NestedProperty", new NumericFilterOperators().Eq(100));

            var query = typeQueryBuilder.ToQuery().GetQuery();

            Assert.NotNull(query);
            Assert.Contains(expectedFields, query.Query);
            Assert.Equal(query.Query, expectedFullQuery);
        }
        [Fact]
        public void generate_where_with_IFilterOperator()
        {
            const string expectedFields = "{items{Property1}}";
            const string expectedFilter = "where:{NestedObjects:{NestedProperty:{eq: 100}}}";
            const string expectedFullQuery = $"RequestTypeObject({expectedFilter}){expectedFields}";

            typeQueryBuilder.Field(x => x.Property1);
            typeQueryBuilder.Where(x => x.NestedObjects, f => f.NestedProperty, new NumericFilterOperators().Eq(100));

            var query = typeQueryBuilder.ToQuery().GetQuery();

            Assert.NotNull(query);
            Assert.Contains(expectedFields, query.Query);
            Assert.Equal(query.Query, expectedFullQuery);
        }
    }
}
