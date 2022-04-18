using System;
using System.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using MESWebAPI.Models;

namespace MESWebAPI.Repositories
{
#nullable disable
    public class BasisRepository : Repository
    {
        public BasisRepository(string connectionString) : base(connectionString)
        { }

        public BasisRepository() : base()
        { }

        public string GetBUBasisConnection(string buName)
        {
            string tSql = @"SELECT TOP 1 a.BasisConnection
                                FROM    dbo.tBUBasisDBConnection a
                                        INNER JOIN dbo.tBU_NBA b ON a.BUID = b.BUID
                                WHERE   b.BUName = @BUName
                                        AND a.BasisConnection <> ''";
            using (IDbConnection conn = Connection)
            {
                conn.Open();
                return conn.QueryFirstOrDefault<string>(tSql, new { BUName = buName });

            }
        }

        public async Task<BUBasisDBConnection> GetBUBasisConnection(int buID)
        {
            string tSql = @"SELECT * FROM tBUBasisDBConnection WHERE BUID=@BUID";
            using (IDbConnection conn = Connection)
            {
                conn.Open();
                return await conn.QueryFirstOrDefaultAsync<BUBasisDBConnection>(tSql, new { BUID = buID });
            }
        }

        public async Task<ProductDBConnection> GetALLProductConnection(int productID)
        {
            string tSql = @"SELECT * FROM tProductDBConnection WHERE ProductID=@ProductID";
            using (IDbConnection conn = Connection)
            {
                conn.Open();
                return await conn.QueryFirstOrDefaultAsync<ProductDBConnection>(tSql, new { ProductID = productID });
            }
        }

        public async Task<ProductDBConnection> GetProductConnection(int productID)
        {
            string tSql = @"SELECT  d.[BasisConnection],d.[RMABasisConnection],a.[ProductID],
		                    c.[MasterDB],c.[MeasureDB],c.[TransactionDB],
		                    c.[LogDB],c.[SNPoolDB],c.[PPSDB],c.[CPKDB] 
	                        FROM tProduct a
	                        INNER JOIN tBU_NBA  b ON a.BUID=b.BUID
	                        INNER JOIN tProductDBConnection c ON a.ProductID=c.ProductID
	                        INNER JOIN tBUBasisDBConnection d ON b.BUID=d.BUID WHERE a.ProductID=@ProductID";
            using (IDbConnection conn = Connection)
            {
                conn.Open();
                return await conn.QueryFirstOrDefaultAsync<ProductDBConnection>(tSql, new { ProductID = productID });
            }
        }

        public async Task<IEnumerable<BU_NBA>> GetBU(bool isEnable = false)
        {
            string tSql = @"SELECT BUID,BUName FROM dbo.tBU_NBA";
            if (isEnable)
            {
                tSql = tSql + " WHERE Enable=1 ";
            }
            tSql = tSql + " ORDER BY BUName ";
            using (IDbConnection conn = Connection)
            {
                conn.Open();
                return await conn.QueryAsync<BU_NBA>(tSql);
            }

        }

        public async Task<IEnumerable<Product>> GetProductByBU(int buid, bool isEnable = false)
        {
            string tSql = @"SELECT ProductID, ProductCode, ProductName, BUID, Enable FROM dbo.tProduct WHERE BUID=@BUID";
            if (isEnable)
            {
                tSql = tSql + " AND Enable=1 ";
            }
            tSql = tSql + " ORDER BY ProductName ";
            using (IDbConnection conn = Connection)
            {
                conn.Open();
                return await conn.QueryAsync<Product>(tSql, new { BUID = buid });
            }
        }

        public IEnumerable<Product> GetProductByBUName(string buName, bool isEnable = false)
        {
            string tSql = @"SELECT ProductID, ProductCode, ProductName, b.BUID, a.Enable  FROM dbo.tProduct a INNER JOIN dbo.tBU_NBA b ON a.BUID = b.BUID WHERE b.BUName=@BUName";
            if (isEnable)
            {
                tSql = tSql + " AND a.Enable=1 ";
            }
            tSql = tSql + " ORDER BY a.ProductName ";
            using (IDbConnection conn = Connection)
            {
                conn.Open();
                return conn.Query<Product>(tSql, new { BUName = buName });
            }
        }

        public async Task<IEnumerable<PDLine>> GetProductLineByBU(int buid, bool isEnable = false)
        {
            string tSql = @"SELECT LineID, RTRIM(LineName) AS LineName, BUID, Enable FROM dbo.tProductLine WHERE BUID=@BUID";
            if (isEnable)
            {
                tSql = tSql + " AND Enable=1 ";
            }
            tSql = tSql + " ORDER BY LineID ";
            using (IDbConnection conn = Connection)
            {
                conn.Open();
                return await conn.QueryAsync<PDLine>(tSql, new { BUID = buid });
            }
        }

        public async Task<IEnumerable<ProcessNOInfo>> GetProcessNOByProduct(int productID, bool isEnable = false)
        {
            string tSql = @"SELECT DISTINCT c.ProcessNO,c.ProcessNOLevel,c.ProcessName,c.Enable FROM dbo.tModel a WITH(NOLOCK)
                            INNER JOIN dbo.tModelProcessNO_MAP b WITH(NOLOCK) ON a.ModelID=b.ModelID
                            INNER JOIN dbo.tProcessNOInfo c WITH(NOLOCK) ON b.ProcessNO=c.ProcessNO
                            WHERE a.ProductID=@ProductID";
            if (isEnable)
            {
                tSql = tSql + " AND a.Enable=1 AND c.Enable=1 ";
            }
            tSql = tSql + " ORDER BY c.ProcessNO ";
            using (IDbConnection conn = Connection)
            {
                conn.Open();
                return await conn.QueryAsync<ProcessNOInfo>(tSql, new { ProductID = productID });
            }
        }

        public async Task<User> GetUser(string userID)
        {
            string tSql = @"SELECT * FROM tUser WHERE UserID=@UserID";
            using (IDbConnection conn = Connection)
            {
                conn.Open();
                return await conn.QueryFirstOrDefaultAsync<User>(tSql, new { UserID = userID });
            }
        }

        public async Task<int> AddPlantBOPMaterial(string iacPartNo, string processno, string modifyUser, int qty)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@IACPartNo", iacPartNo);
            dp.Add("@ProcessNO", processno);
            dp.Add("@ModifyUser", modifyUser);
            dp.Add("@Qty", qty);

            using (IDbConnection conn = Connection)
            {
                conn.Open();
                return await conn.ExecuteScalarAsync<int>("usp_ZTABLE_PlantBOP_KeyPartProcessNO_Insert", dp, commandType: CommandType.StoredProcedure);
            }
        }

    }
}