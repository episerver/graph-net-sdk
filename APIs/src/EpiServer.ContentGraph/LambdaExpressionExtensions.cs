using EPiServer.ContentGraph.Connection;
using System;
using System.Linq.Expressions;
using System.Threading;

namespace EPiServer.ContentGraph
{
    public static class LambdaExpressionExtensions
    {
        private static Lazy<ExpressionHashCalculator> _expressionHashCalculator = new Lazy<ExpressionHashCalculator>(() => new ExpressionHashCalculator());
        private const string _prefixCacheName = "LambdaExpressionExtensions:";
        private const int DEFAULT_SLIDING_EXPIRATION_IN_MINUTES = 5;
        private static ICache _cache;
        private static StaticCachePolicy _cachePolicy;
        private static long _compiles = 0;
        private static long _calls = 0;

        public static ICache Cache
        {
            get
            {
                return _cache ?? (_cache = CacheAccessor.Cache);
            }
            set
            {
                _cache = value;
            }
        }

        public static StaticCachePolicy CachePolicy
        {
            get
            {
                return _cachePolicy ?? (_cachePolicy = new StaticCachePolicy(TimeSpan.FromMinutes(DEFAULT_SLIDING_EXPIRATION_IN_MINUTES)));
            }
            set
            {
                _cachePolicy = value;
            }
        }

        public static long CachedCompileCompilations
        {
            get
            {
                return _compiles;
            }
        }

        public static long CachedCompileCalls
        {
            get
            {
                return _calls;
            }
        }

        public static Delegate CachedCompile(this LambdaExpression expression, out object[] constants)
        {
            Interlocked.Increment(ref _calls);

            var expr = new ExpressionConstantExtractor(expression).ReplaceConstants(out constants);

            var cacheKey = GetCacheKey(expr, constants);
            var cachedFunction = Cache.Get<Delegate>(cacheKey);
            if(cachedFunction != default(Delegate))
            {
                return cachedFunction;
            }

            Interlocked.Increment(ref _compiles);
            cachedFunction = expr.Compile();
            Cache.AddOrUpdate(cacheKey, CachePolicy, cachedFunction);

            return cachedFunction;
        }

        private static string GetCacheKey(Expression expression, object[] constants)
        {
            return $"{_prefixCacheName}_{_expressionHashCalculator.Value.CalculateHashCode(expression)}";
        }

        internal static T CachedCompileInvoke<T>(this Expression expression)
        {
            var lambdaExpression = Expression.Lambda(expression);

            object[] constants;
            var functionDelegate = lambdaExpression.CachedCompile(out constants);

            T returnObject;
            if (TryInvoke(functionDelegate, constants, out returnObject))
            {
                return returnObject;
            }

            return ((dynamic)functionDelegate)(constants);
        }

        internal static object CachedCompileInvoke(this Expression expression)
        {
            var lambdaExpression = Expression.Lambda(expression);

            object[] constants;
            var functionDelegate = lambdaExpression.CachedCompile(out constants);
            return ((dynamic)functionDelegate)(constants);

        }

        private static bool TryInvoke<T>(Delegate functionDelegate, object[] constants, out T obj)
        {
            var functionObject = functionDelegate as Func<object[], T>;
            if (functionObject != null)
            {
                obj = functionObject(constants);
                return true;
            }

            obj = default(T);
            return false;
        }
    }
}
