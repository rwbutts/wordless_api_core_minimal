namespace WordlessApi 
{
    interface IWordlessApi 
    {
        QueryMatchCountResponse CountMatches(string answer, IEnumerable<string> guesses );
        GetWordResponse TodaysWord( int dayIndex );
        WordExistsResponse WordExists( string word );
        GetWordResponse RandomWord();
        HealthCheckResponse HealthCheck();

    }
}
