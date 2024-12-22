using CompetenceForm.Common;
using MediatR;

namespace CompetenceForm.Queries
{
    public class GetUnfinishedUserCountQuery : IRequest<ServiceResult<int>>{}

}
