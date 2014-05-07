using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;

namespace DataAccess.Mappers
{
    internal abstract class ASQLMapper<TEntity> where TEntity : Entity 
    {
        protected Dictionary<int, TEntity> entityMap;
        protected string connectionString;

        protected abstract string insertProcedureName { get; }
        protected abstract string selectAllProcedureName { get; }
        protected abstract string updateProcedureName { get; }

        protected abstract TEntity entityFromReader(SqlDataReader reader);

        protected abstract void addInsertParameters(TEntity entity,
            SqlParameterCollection parameters);
        protected abstract void addUpdateParameters(TEntity entity,
            SqlParameterCollection parameters);

        protected List<TEntity> selectAll()
        {
            List<TEntity> entities = new List<TEntity>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = selectAllProcedureName;

                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TEntity entity = entityFromReader(reader);

                            if (!entityMap.ContainsKey(entity.Id))
                            {
                                entityMap.Add(entity.Id, entity);
                            }
                            else
                            {
                                // NOTE: This means data in cache is not overwritten!
                                // Also means you can't discard changes!
                                // Either provide the Mapper class with Flush(entity) and FlushAll()
                                // metods, or provide a 'overwrite' bool to Read statements, which
                                // if true will overwrite what is in cache, so that changes may
                                // be discarded
                                entity = entityMap[entity.Id];
                            }

                            if (entity.Deleted == false)
                            {
                                entities.Add(entity);
                            }
                        }
                    }
                }
            }

            return entities;
        }
    }
}
