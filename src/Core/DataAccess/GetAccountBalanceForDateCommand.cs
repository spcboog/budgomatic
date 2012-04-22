using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Criterion;
using Budgomatic.Core.Domain;

namespace Budgomatic.Core.DataAccess
{
    public class GetAccountBalanceForDateCommand : IGetAccountBalanceForDateCommand
    {
        private IRepository _repository;
        private ISessionFactory _sessionFactory;

        public GetAccountBalanceForDateCommand(
            IRepository repository,
            ISessionFactory sessionFactory)
        {
            _repository = repository;
            _sessionFactory = sessionFactory;
        }

        private decimal GetSum(ISession session, Guid accountId, DateTime date, EntryType entryType)
        {
            // TODO : try to get the lamda version working
            // currently gives this error: "could not resolve property" regarding Transaction.Date

            //return session.QueryOver<Entry>().Where(x =>
            //    (x.Account.Id == accountId) &&
            //    (x.Transaction.Date <= date) &&
            //    (x.Type == entryType))
            //    .List()
            //    .Sum(x => x.Amount);

            var entries = session.CreateCriteria<Entry>()
                .Add(Expression.Eq("Type", entryType))
                .Add(Expression.Eq("Account.Id", accountId))
                .CreateCriteria("Transaction")
                    .Add(Expression.Eq("Deleted", false))
                    .Add(Expression.Le("Date", date))
                    .List();

            decimal sum = 0;
            foreach (Entry entry in entries)
            {
                sum += entry.Amount;
            }
            return sum;
        }

        public decimal Execute(Guid accountId, DateTime date)
        {
            var account = _repository.Find<Account>(accountId);

            using (var session = _sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var debitSum = GetSum(session, accountId, date, EntryType.Debit);
                    var creditSum = GetSum(session, accountId, date, EntryType.Credit);

                    if (account is DebitIncreaseAccount)
                    {
                        return debitSum - creditSum;
                    }
                    else
                    {
                        return creditSum - debitSum;
                    }
                }
            }
        }
    }
}
