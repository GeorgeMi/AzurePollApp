using System.Collections.Generic;

namespace DataTransferObject
{
    public class VoteResultDTO
    {
       public List<VoteQuestionResultDTO> Questions { get; set; }
       public int NrVotes { get; set; }
    }
}
