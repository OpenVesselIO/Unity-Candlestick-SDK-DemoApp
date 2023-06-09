namespace Candlestick
{
    public enum CandlestickEnvironment
    {
        /**
         * This is the primary (production) environment.
        */
        Production,

        /**
         * This is a staging (test) environment. Use it to test the logic of your app.
         */
        Staging,

        /**
         * This is an internal development environment. It is reserved for internal development only
         */
        Development,
    }
}