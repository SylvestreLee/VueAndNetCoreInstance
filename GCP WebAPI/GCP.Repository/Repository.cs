using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FreeSql;

namespace GCP.Repository
{
    public class RepositoryBase<TEntity, TKey> : BaseRepository<TEntity, TKey> where TEntity : class, new()
    {
        public RepositoryBase(IFreeSql freeSql) : base(freeSql, null, null)
        {

        }

        public virtual Task<TDto> GetAsync<TDto>(TKey id)
        {
            return Select.WhereDynamic(id).ToOneAsync<TDto>();
        }

        public virtual Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> exp)
        {
            return Select.Where(exp).ToOneAsync();
        }

        public virtual Task<TDto> GetAsync<TDto>(Expression<Func<TEntity, bool>> exp)
        {
            return Select.Where(exp).ToOneAsync<TDto>();
        }

        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteLogicallyAsync(TKey id)
        {
            try
            {
                await UpdateDiy
                    .SetDto(new
                    {
                        Status = 2
                    })
                    .WhereDynamic(id)
                    .ExecuteAffrowsAsync();
            }
            catch (Exception err)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<bool> DeleteLogicallyAsync(TKey[] ids)
        {
            try
            {
                await UpdateDiy
                .SetDto(new
                {
                    Status = 2
                })
                .WhereDynamic(ids)
                .ExecuteAffrowsAsync();
            }
            catch (Exception err)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 更新不是NULL的字段
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        public async Task<int> UpdateNotNullAsync(TEntity entity)
        {
            try
            {
                return await UpdateDiy
                    .SetSourceIgnore(entity, t => t == null)
                    .ExecuteAffrowsAsync();
            }
            catch (Exception err)
            {
                return 0;
            }
        }

        /// <summary>
        /// 更新指定字段
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        public async Task<int> UpdateColumnsAsync(TEntity entity, Expression<Func<TEntity, object>> columns)
        {
            try
            {
                return await UpdateDiy
                    .SetSource(entity)
                    .UpdateColumns(columns)
                    .ExecuteAffrowsAsync();
            }
            catch (Exception err)
            {
                return 0;
            }
        }

        /// <summary>
        /// 更新并且忽略指定字段
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public async Task<int> UpdateIgnoreColumnsAsync(TEntity entity, Expression<Func<TEntity, object>> columns)
        {
            try
            {
                return await UpdateDiy
                    .SetSource(entity)
                    .IgnoreColumns(columns)
                    .ExecuteAffrowsAsync();
            }
            catch (Exception err)
            {
                return 0;
            }
        }
    }
    public class RepositoryBase<TEntity> : RepositoryBase<TEntity, long> where TEntity : class, new()
    {
        public RepositoryBase() : base(Sql.GetFsql())
        {

        }
    }
}
