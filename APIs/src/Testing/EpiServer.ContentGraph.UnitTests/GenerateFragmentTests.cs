using EpiServer.ContentGraph.UnitTests.QueryTypeObjects;
using EPiServer.ContentGraph.Api.Querying;
using Xunit;

namespace EpiServer.ContentGraph.UnitTests
{
    public class GenerateFragmentTests
    {
        [Fact]
        public void set_name_for_fragment_should_generate_correct_name()
        {
            const string fragmentName = "my_fragment";
            FragmentBuilder fragmentBuilder = new FragmentBuilder();
            fragmentBuilder.OperationName(fragmentName);

            Assert.True(fragmentBuilder.GetName().Equals(fragmentName));
        }
        [Fact]
        public void select_3_fields_should_generate_correct_fields()
        {
            const string fragmentName = "my_fragment";
            const string expectedFragmment = "fragment my_fragment on RequestTypeObject {Property1 Property2 Property3{NestedProperty}}";
            FragmentBuilder<RequestTypeObject> fragmentBuilder = new FragmentBuilder<RequestTypeObject>();
            fragmentBuilder.OperationName(fragmentName);
            fragmentBuilder.Fields(x => x.Property1,x=>x.Property2,x=>x.Property3.NestedProperty);

            string? query = fragmentBuilder.GetQuery().Query;
            Assert.Equal(query, expectedFragmment);
        }
        [Fact]
        public void add_fragment_to_a_type_query_should_generate_correct_string()
        {
            const string expectedFields = "{items{Property1 ...my_fragment}}";
            const string fragmentName = "my_fragment";
            const string expectedFragmment = "fragment my_fragment on RequestTypeObject {Property1 Property2 Property3{NestedProperty}}";

            FragmentBuilder<RequestTypeObject> fragmentBuilder = new FragmentBuilder<RequestTypeObject>();
            fragmentBuilder.OperationName(fragmentName);
            fragmentBuilder.Fields(x => x.Property1, x => x.Property2, x => x.Property3.NestedProperty);

            GraphQueryBuilder graphQueryBuilder = new GraphQueryBuilder();
            graphQueryBuilder
                    .ForType<RequestTypeObject>()
                        .Field(x=>x.Property1)
                        .Fragments(fragmentBuilder)
                    .ToQuery()
                .BuildQueries();

            Assert.Equal(graphQueryBuilder.GetFragments().First().GetName(), fragmentName);
            Assert.Contains(expectedFragmment, graphQueryBuilder.GetQuery().Query);
            Assert.Contains(expectedFields, graphQueryBuilder.GetQuery().Query);
        }
    }
}
