using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DataAccess.Entities;

namespace DataAccess.Mappers
{
    internal abstract class ASQLMapper<TEntity> where TEntity : Entity
    {
        #region Protected Fields
        protected Dictionary<int, TEntity> entityMap;
        protected string connectionString;
        #endregion

        #region Protected Properties
        protected abstract string insertProcedureName { get; }
        protected abstract string selectAllProcedureName { get; }
        protected abstract string updateProcedureName { get; }
        #endregion

        #region Protected Methods
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

                    SqlParameter parameter = new SqlParameter("@Id", entity.Id);
                    parameter.Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters.Add(parameter);

                    parameter = new SqlParameter("@LastModified", entity.LastModified);
                    parameter.Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters.Add(parameter);

                    parameter = new SqlParameter("@Deleted", entity.Deleted);
                    cmd.Parameters.Add(parameter);

                    addInsertParameters(entity, cmd.Parameters);

                    con.Open();
                    cmd.ExecuteNonQuery();

                    entity.Id = (int)cmd.Parameters["@Id"].Value;
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

                    SqlParameter parameter = new SqlParameter("@Id", entity.Id);
                    cmd.Parameters.Add(parameter);

                    parameter = new SqlParameter("@LastModified", entity.LastModified);
                    parameter.Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters.Add(parameter);

                    parameter = new SqlParameter("@Deleted", entity.Deleted);
                    cmd.Parameters.Add(parameter);

                    addUpdateParameters(entity, cmd.Parameters);

                    con.Open();
                    cmd.ExecuteNonQuery();

                    entity.LastModified = (DateTime)cmd.Parameters["@LastModified"].Value;
                }
            }

            return entity;
        }
        #endregion
    }
}
