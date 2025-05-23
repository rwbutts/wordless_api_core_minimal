namespace WordlessApi 
{
    interface IWordlessApi 
    {
        QueryMatchCountResponse CountMatches( QueryMatchCountRequest request );
        GetWordResponse TodaysWord( int dayIndex );
        WordExistsResponse WordExists( string word );
        GetWordResponse RandomWord();
        HealthCheckResponse HealthCheck();
        string GetAssemblyVersionString();

    }
}
