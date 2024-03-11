
using EPiServer.ContentGraph.Helpers;

namespace EPiServer.ContentGraph.Api.Filters
{
    public class NumericFilterOperators : IFilterOperator
    {
        string _query = string.Empty;

        public string Query => _query;
        public static NumericFilterOperators Create()
        {
            return new NumericFilterOperators();
        }
        public NumericFilterOperators Boost(int value)
        {
            _query += _query.IsNullOrEmpty() ? $"boost: {value}" : $",boost: {value}";
            return this;
        }
        public NumericFilterOperators Eq(int value)
        {
            _query += _query.IsNullOrEmpty() ? $"eq: {value}" : $",eq: {value}";
            return this;
        }
        public NumericFilterOperators NotEq(int value)
        {
            _query += _query.IsNullOrEmpty() ? $"notEq: {value}" : $",notEq: {value}";
            return this;
        }
        public NumericFilterOperators Gt(int value)
        {
            _query += _query.IsNullOrEmpty() ? $"gt: {value}" : $",gt: {value}";
            return this;
        }
        public NumericFilterOperators Gte(int value)
        {
            _query += _query.IsNullOrEmpty() ? $"gte: {value}" : $",gte: {value}";
            return this;
        }
        public NumericFilterOperators Lt(int value)
        {
            _query += _query.IsNullOrEmpty() ? $"lt: {value}" : $",lt: {value}";
            return this;
        }
        public NumericFilterOperators Lte(int value)
        {
            _query += _query.IsNullOrEmpty() ? $"lte: {value}" : $",lte: {value}";
            return this;
        }
        public NumericFilterOperators Exists(bool value)
        {
            _query += _query.IsNullOrEmpty() ? $"exist: {value.ToString().ToLower()}" : $",exist: {value.ToString().ToLower()}";
            return this;
        }
        public NumericFilterOperators In(params int[] values)
        {
            _query += _query.IsNullOrEmpty() ? $"in: [{string.Join(',', values)}]" : $",in: [{string.Join(',', values)}]";
            return this;
        }
        public NumericFilterOperators NotIn(params int[] values)
        {
            _query += _query.IsNullOrEmpty() ? $"notIn: [{string.Join(',', values)}]" : $",notIn: [{string.Join(',', values)}]";
            return this;
        }
        public NumericFilterOperators InRange(int? from, int? to)
        {
            if (from.HasValue)
            {
                Gte(from.Value);
            }
            if (to.HasValue)
            {
                Lte(to.Value);
            }
            return this;
        }
        public NumericFilterOperators InRanges(params (int? from, int? to)[] ranges)
        {
            foreach (var range in ranges)
            {
                InRange(range.from, range.to);
            }
            return this;
        }

        public NumericFilterOperators Eq(float value)
        {
            _query += _query.IsNullOrEmpty() ? $"eq: {value}" : $",eq: {value}";
            return this;
        }
        public NumericFilterOperators NotEq(float value)
        {
            _query += _query.IsNullOrEmpty() ? $"notEq: {value}" : $",notEq: {value}";
            return this;
        }
        public NumericFilterOperators Gt(float value)
        {
            _query += _query.IsNullOrEmpty() ? $"gt: {value}" : $",gt: {value}";
            return this;
        }
        public NumericFilterOperators Gte(float value)
        {
            _query += _query.IsNullOrEmpty() ? $"gte: {value}" : $",gte: {value}";
            return this;
        }
        public NumericFilterOperators Lt(float value)
        {
            _query += _query.IsNullOrEmpty() ? $"lt: {value}" : $",lt: {value}";
            return this;
        }
        public NumericFilterOperators Lte(float value)
        {
            _query += _query.IsNullOrEmpty() ? $"lte: {value}" : $",lte: {value}";
            return this;
        }
        public NumericFilterOperators Gt(double value)
        {
            _query += _query.IsNullOrEmpty() ? $"gt: {value}" : $",gt: {value}";
            return this;
        }
        public NumericFilterOperators Gte(double value)
        {
            _query += _query.IsNullOrEmpty() ? $"gte: {value}" : $",gte: {value}";
            return this;
        }
        public NumericFilterOperators Lt(double value)
        {
            _query += _query.IsNullOrEmpty() ? $"lt: {value}" : $",lt: {value}";
            return this;
        }
        public NumericFilterOperators Lte(double value)
        {
            _query += _query.IsNullOrEmpty() ? $"lte: {value}" : $",lte: {value}";
            return this;
        }
        public NumericFilterOperators In(params float[] values)
        {
            _query += _query.IsNullOrEmpty() ? $"in: [{string.Join(',', values)}]" : $",in: [{string.Join(',', values)}]";
            return this;
        }
        public NumericFilterOperators NotIn(params float[] values)
        {
            _query += _query.IsNullOrEmpty() ? $"notIn: [{string.Join(',', values)}]" : $",notIn: [{string.Join(',', values)}]";
            return this;
        }
        public NumericFilterOperators InRange(float? from, float? to)
        {
            if (from.HasValue)
            {
                Gte(from.Value);
            }
            if (to.HasValue)
            {
                Lte(to.Value);
            }
            return this;
        }
        public NumericFilterOperators InRange(double? from, double? to)
        {
            if (from.HasValue)
            {
                Gte(from.Value);
            }
            if (to.HasValue)
            {
                Lte(to.Value);
            }
            return this;
        }
        public NumericFilterOperators InRanges(params (float? from, float? to)[] ranges)
        {
            foreach (var range in ranges)
            {
                InRange(range.from, range.to);
            }
            return this;
        }
        public NumericFilterOperators InRanges(params (double? from, double? to)[] ranges)
        {
            foreach (var range in ranges)
            {
                InRange(range.from, range.to);
            }
            return this;
        }
    }
}
