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
        [Fact]
        public void multiple_fragments_should_generate_correct_query()
        {
            const string expectedMainQuery = "query FragmentTest {FragmentObject{items{Name ...FirstFragment ...SecondFragment}}}";
            const string expectedFistFragment = "fragment FirstFragment on PromoObject {ProviderName}";
            const string expectedSecondFragmment = "fragment SecondFragment on FragmentObject {ViewingTime PromoImage{Url}}";
            const string expectedFullQuery = $"{expectedMainQuery}\n{expectedFistFragment}\n{expectedSecondFragmment}";

            var firstFragment = new FragmentBuilder<PromoObject>("FirstFragment");
            firstFragment.Fields(x => x.ProviderName);

            var secondFragment = new FragmentBuilder<FragmentObject>("SecondFragment");
            secondFragment.Fields(x => x.ViewingTime, x=> x.PromoImage.Url);

            GraphQueryBuilder graphQueryBuilder = new GraphQueryBuilder();
            graphQueryBuilder
                .OperationName("FragmentTest")
                    .ForType<FragmentObject>()
                        .Field(x => x.Name)
                        .Fragments(firstFragment, secondFragment)
                    .ToQuery()
                .BuildQueries();

            Assert.Equal(graphQueryBuilder.GetFragments().First().GetName(), "FirstFragment");
            Assert.Equal(graphQueryBuilder.GetFragments().First().GetQuery().Query, expectedFistFragment);

            Assert.Equal(graphQueryBuilder.GetFragments().Last().GetName(), "SecondFragment");
            Assert.Equal(graphQueryBuilder.GetFragments().Last().GetQuery().Query, expectedSecondFragmment);

            Assert.Equal(expectedFullQuery, graphQueryBuilder.GetQuery().Query);
        }
        [Fact]
        public void nested_fragments_should_generate_correct_query()
        {
            const string expectedMainQuery = "query FragmentTest {FragmentObject{items{Name ...SecondFragment}}}";
            const string expectedFistFragment = "fragment FirstFragment on PromoObject {Expanded{Property1}}";
            const string expectedSecondFragmment = "fragment SecondFragment on FragmentObject {PromoText PromoImage{Url} ...FirstFragment}";
            const string expectedFullQuery = $"{expectedMainQuery}\n{expectedSecondFragmment}\n{expectedFistFragment}";

            var firstFragment = new FragmentBuilder<PromoObject>("FirstFragment");
            firstFragment.Fields(x => x.Expanded.Property1);

            var secondFragment = new FragmentBuilder<FragmentObject>("SecondFragment");
            secondFragment.Fields(x => x.PromoText, x=> x.PromoImage.Url);
            secondFragment.Fragments(firstFragment);

            GraphQueryBuilder graphQueryBuilder = new GraphQueryBuilder();
            graphQueryBuilder
                .OperationName("FragmentTest")
                    .ForType<FragmentObject>()
                        .Field(x => x.Name)
                        .Fragments(secondFragment)
                    .ToQuery()
                .BuildQueries();

            //expect children in secondary fragment
            Assert.Equal(graphQueryBuilder.GetFragments().First().GetName(), "SecondFragment");
            Assert.True(graphQueryBuilder.GetFragments().First().HasChildren);
            Assert.Equal(graphQueryBuilder.GetFragments().First().ChildrenFragments.First().GetQuery().Query, expectedFistFragment);
            //expect secondary fragment
            Assert.Equal(graphQueryBuilder.GetFragments().First().GetQuery().Query, expectedSecondFragmment);
            //expect full query
            Assert.Equal(expectedFullQuery, graphQueryBuilder.GetQuery().Query);
        }
    }
}
