namespace WordlessApi {

    public enum ScoreCode{ unset=0, not_present, is_elsewhere, correct };

    public class GuessScores
    {
        private string _guessWord;
        private readonly List<ScoreCode> _scoreCodes;

        public List<ScoreCode> ScoreCodes { get => _scoreCodes; }
        public string GuessWord { get => _guessWord; }

        protected GuessScores(string guessWord, string answerWord)
        {
            if(String.IsNullOrEmpty(guessWord))
                throw new ArgumentNullException(nameof(guessWord));

            if(String.IsNullOrEmpty(answerWord))
                throw new ArgumentNullException(nameof(answerWord));
                
            if(guessWord.Length != answerWord.Length)
                throw new ArgumentException("guessWord and answerWord strings have unequal lengths");
                
            this._guessWord = guessWord;
            _scoreCodes = GetScoreCodes(guessWord, answerWord);
        }

        private static List<ScoreCode> GetScoreCodes( string guessWord, string answerWord )
        {
            List<ScoreCode> codes = [];
            for( int i=0; i< guessWord.Length; i++ )
            {
                codes.Add(GetScoreCode(guessWord, answerWord, i));
            }
            return codes;
    }

        private static ScoreCode GetScoreCode( string guessWord, string answerWord, int position )
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
                return ScoreCode.is_elsewhere;
            }
            else
            {
                return ScoreCode.not_present;
            }
        }

        public bool GuessScoresIdenticalAgainst( string alternateAnswer )
        {
            GuessScores testScore = CreateGuessScores(this._guessWord, alternateAnswer);
            return _scoreCodes.SequenceEqual(testScore._scoreCodes);
        }

        public static GuessScores CreateGuessScores(string guess, string answer )
        {
            return new GuessScores(guess,answer);
        }
    }
}