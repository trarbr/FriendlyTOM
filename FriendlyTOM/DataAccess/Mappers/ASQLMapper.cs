/*
Copyright 2014 The Friendly TOM Team (see AUTHORS.rst)

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

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

        protected abstract TEntity entityFromReader(SqlDataReader reader);

        protected abstract void addInsertParameters(TEntity entity,
            SqlParameterCollection parameters);
        protected abstract void addUpdateParameters(TEntity entity,
            SqlParameterCollection parameters);

        #region SelectAll
        protected List<TEntity> selectAll()
        {
            List<TEntity> entities = new List<TEntity>();

            //Opens Data Connection
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    //Finds the selectAllProcedure and executes it.
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = selectAllProcedureName;

                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //Then reads from the database if the List is empty.
                            TEntity entity = entityFromReader(reader);

                            if (!entityMap.ContainsKey(entity.Id))
                            {
                                entityMap.Add(entity.Id, entity);
                            }
                                //If not empty, reads from the cache. 
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
        #endregion

        #region Insert
        protected TEntity insert(TEntity entity)
        {
            //Opens connection
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    //Finds insertprocedure and executes it.
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
                    //Closes the database when leaving the using statement
                }
            }

            return entity;
        }
        #endregion

        #region Update
        protected TEntity update(TEntity entity)
        {
            //Opens the connection
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    //Executes storedprocedure.
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
                    //Closes database.
                }
            }

            return entity;
        }
        #endregion
    }
}
