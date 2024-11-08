using EPiServer.ContentGraph.Api.Querying;
using EPiServer.ContentGraph.ExpressionHelper;
using EPiServer.ContentGraph.Extensions;
using Xunit;

namespace EpiServer.ContentGraph.UnitTests.QueryTypeObjects
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
                        .AddFragments(fragmentBuilder)
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
                        .AddFragments(firstFragment, secondFragment)
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
            secondFragment.AddFragments(firstFragment);

            GraphQueryBuilder graphQueryBuilder = new GraphQueryBuilder();
            graphQueryBuilder
                .OperationName("FragmentTest")
                    .ForType<FragmentObject>()
                        .Field(x => x.Name)
                        .AddFragments(secondFragment)
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

        [Fact]
        public void fragment_with_inlinefragment_should_generate_correct_query()
        {
            const string expectedMainQuery = "query FragmentTest {FragmentObject{items{Name ...SecondFragment}}}";
            const string expectedFistFragment = "fragment FirstFragment on PromoObject {Expanded{Property1} ... on PromoExtend{ProviderName}}";
            const string expectedSecondFragmment = "fragment SecondFragment on FragmentObject {PromoText PromoImage{Url} ...FirstFragment}";
            const string expectedFullQuery = $"{expectedMainQuery}\n{expectedSecondFragmment}\n{expectedFistFragment}";

            var firstFragment = new FragmentBuilder<PromoObject>("FirstFragment");
            firstFragment.Fields(x => x.Expanded.Property1);
            firstFragment.Inline<PromoExtend>(x => x.ProviderName);

            Assert.Equal(firstFragment.GetQuery().Query, expectedFistFragment);

            var secondFragment = new FragmentBuilder<FragmentObject>("SecondFragment");
            secondFragment.Fields(x => x.PromoText, x => x.PromoImage.Url);
            secondFragment.AddFragments(firstFragment);

            GraphQueryBuilder graphQueryBuilder = new GraphQueryBuilder();
            graphQueryBuilder
                .OperationName("FragmentTest")
                    .ForType<FragmentObject>()
                        .Field(x => x.Name)
                        .AddFragments(secondFragment)
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

        [Fact]
        public void fragment_with_single_recursive_should_generate_correct_query()
        {
            const string expectedMainQuery = "query FragmentTest {FragmentObject{items{Name ...SecondFragment}}}";
            const string expectedFistFragment = "fragment FirstFragment on PromoObject {Expanded{Property1} ... on PromoExtend{Details{CompanyContentLink @recursive(depth:10)}}}";
            const string expectedSecondFragmment = "fragment SecondFragment on FragmentObject {PromoText PromoImage{Url} ...FirstFragment}";
            const string expectedFullQuery = $"{expectedMainQuery}\n{expectedSecondFragmment}\n{expectedFistFragment}";

            var firstFragment = new FragmentBuilder<PromoObject>("FirstFragment");
            firstFragment.Fields(x => x.Expanded.Property1);
            firstFragment.Recursive<PromoExtend>(x => x.Details.CompanyContentLink, 10);

            Assert.Equal(firstFragment.GetQuery().Query, expectedFistFragment);

            var secondFragment = new FragmentBuilder<FragmentObject>("SecondFragment");
            secondFragment.Fields(x => x.PromoText, x => x.PromoImage.Url);
            secondFragment.AddFragments(firstFragment);

            GraphQueryBuilder graphQueryBuilder = new GraphQueryBuilder();
            graphQueryBuilder
                .OperationName("FragmentTest")
                    .ForType<FragmentObject>()
                        .Field(x => x.Name)
                        .AddFragments(secondFragment)
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

        [Fact]
        public void fragment_on_field_should_generate_correct_query()
        {
            const string expectedMainQuery = "query FragmentTest {FragmentObject{items{Name PromoImage{...SecondFragment}}}}";
            const string expectedFistFragment = "fragment FirstFragment on PromoExtend {ProviderName}";
            const string expectedSecondFragmment = "fragment SecondFragment on PromoObject {Url PromoExtend{...FirstFragment}}";
            const string expectedFullQuery = $"{expectedMainQuery}\n{expectedSecondFragmment}\n{expectedFistFragment}";

            var firstFragment = new FragmentBuilder<PromoExtend>("FirstFragment");
            firstFragment.Fields(x => x.ProviderName);

            Assert.Equal(firstFragment.GetQuery().Query, expectedFistFragment);

            var secondFragment = new FragmentBuilder<PromoObject>("SecondFragment");
            secondFragment.Fields(x => x.Url);
            secondFragment.AddFragment(x => x.PromoExtend, firstFragment);

            GraphQueryBuilder graphQueryBuilder = new GraphQueryBuilder();
            graphQueryBuilder
                .OperationName("FragmentTest")
                    .ForType<FragmentObject>()
                        .Field(x => x.Name)
                        .AddFragment(x=> x.PromoImage, secondFragment)
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
        [Fact]
        public void cyclic_fragments_should_generate_correct_query()
        {
            const string expectedMainQuery = "query FragmentTest {FragmentObject{items{Name ...FirstFragment ...SecondFragment}}}";
            const string expectedFistFragment = "fragment FirstFragment on PromoExtend {ProviderName Details{PromoImage{PromoExtend{...FirstFragment}}}}";
            const string expectedSecondFragmment = "fragment SecondFragment on PromoObject {Url PromoExtend{...ThirdFragment}}";
            const string expectedThirdFragmment = "fragment ThirdFragment on PromoExtend {ProviderName PromoExtend{...SecondFragment}}";
            const string expectedFullQuery = $"{expectedMainQuery}\n{expectedFistFragment}\n{expectedSecondFragmment}\n{expectedThirdFragmment}";
            //Self-referenced query: a -> a
            var firstFragment = new FragmentBuilder<PromoExtend>("FirstFragment");
            firstFragment.Fields(x => x.ProviderName);
            firstFragment.AddFragment(x => x.Details.PromoImage.PromoExtend, firstFragment);

            Assert.Equal(firstFragment.GetQuery().Query, expectedFistFragment);

            //Cross-referenced: a -> b -> a
            var secondFragment = new FragmentBuilder<PromoObject>("SecondFragment");
            secondFragment.Fields(x => x.Url);

            var thirdFragment = new FragmentBuilder<PromoExtend>("ThirdFragment");
            thirdFragment.Fields(x => x.ProviderName);

            secondFragment.AddFragment(x => x.PromoExtend, thirdFragment);
            thirdFragment.AddFragment(x=> x.PromoExtend, secondFragment);

            Assert.Equal(secondFragment.GetQuery().Query, expectedSecondFragmment);
            Assert.Equal(thirdFragment.GetQuery().Query, expectedThirdFragmment);

            GraphQueryBuilder graphQueryBuilder = new GraphQueryBuilder();
            graphQueryBuilder
                .OperationName("FragmentTest")
                    .ForType<FragmentObject>()
                        .Field(x => x.Name)
                        .AddFragments(firstFragment, secondFragment)
                    .ToQuery()
                .BuildQueries();

            Assert.Equal(graphQueryBuilder.GetFragments().First().GetQuery().Query, expectedFistFragment);
            Assert.Equal(graphQueryBuilder.GetFragments().Skip(1).First().GetQuery().Query, expectedSecondFragmment);
            Assert.Equal(graphQueryBuilder.GetFragments().Last().GetQuery().Query, expectedThirdFragmment);

            //expect full query
            Assert.Equal(expectedFullQuery, graphQueryBuilder.GetQuery().Query);
        }
    }
}
