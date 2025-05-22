namespace WordlessApi {

    public enum ScoreCode{ unset=0, miss, elsewhere, correct };

    public class GuessScores
    {
        private string _guessWord;
        private List<ScoreCode> _scoreCodes;

        public List<ScoreCode> MatchCodes { get => _scoreCodes; }
        public string GuessWord { get => _guessWord; }

        protected GuessScores(string guessWord, string answerWord)
        {
            if(String.IsNullOrEmpty(guessWord))
                throw new ArgumentNullException("guessWord");

            if(String.IsNullOrEmpty(answerWord))
                throw new ArgumentNullException("answerWord");
                
            if(guessWord.Length != answerWord.Length)
                throw new ArgumentException("guessWord and answerWord strings have unequal lengths");
                
            this._guessWord = guessWord;
            _scoreCodes = ComputeMatchCodes(guessWord, answerWord);
        }

        private List<ScoreCode> ComputeMatchCodes( string guessWord, string answerWord )
        {
            List<ScoreCode> codes = new();
            for( int i=0; i< guessWord.Length; i++ )
            {
                codes.Add(ComputeMatchCode(guessWord, answerWord, i));
            }
            return codes;
    }

        private ScoreCode ComputeMatchCode( string guessWord, string answerWord, int position )
        {
            if(position >= guessWord.Length)
            {
                throw new ArgumentException("index past end of string");
            }

            char guessChar = guessWord[position];

            if(guessChar == answerWord[position])
            {
                return ScoreCode.correct;
            }
            else if(answerWord.Contains(guessChar))
            {
                return ScoreCode.elsewhere;
            }
            else
            {
                return ScoreCode.miss;
            }
        }

        public bool GuessScoresIdenticalAgainst( string testAnswer )
        {
            GuessScores testScore = ComputeScores(this._guessWord, testAnswer);
            return this.CompareScores( testScore );
        }

        public bool CompareScores(GuessScores otherScores)
        {
            return _scoreCodes.SequenceEqual(otherScores._scoreCodes);
        }

        // public override int GetHashCode()
        // {
        //     return _scoreCodes.Aggregate(0, (hash, member) =>
        //         HashCode.Combine(hash, member));
        // }

        public static GuessScores ComputeScores(string guess, string answer )
        {
            return new GuessScores(guess,answer);
        }
    }
}