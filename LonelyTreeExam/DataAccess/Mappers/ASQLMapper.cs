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

        protected TEntity insert(TEntity entity)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = insertProcedureName;
                    addInsertParameters(entity, cmd.Parameters);

                    con.Open();
                    cmd.ExecuteNonQuery();

                    entity.Id = (int)cmd.Parameters["@Id"].Value; // Findes ikke i Payment tabellen! Omdøb PaymentId?
                    entity.LastModified = (DateTime)cmd.Parameters["@LastModified"].Value;

                    entityMap.Add(entity.Id, entity);
                }
            }

            return entity;
        }

        protected TEntity update(TEntity entity)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = updateProcedureName;
                    addUpdateParameters(entity, cmd.Parameters);

                    con.Open();
                    cmd.ExecuteNonQuery();

                    entity.LastModified = (DateTime)cmd.Parameters["@LastModified"].Value;
                }
            }

            return entity;
        }
    }
}
