using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace _24hplusdotnetcore.Services
{
    public class MobileVersionServices : IScopedLifetime
    {
        private readonly ILogger<MobileVersionServices> _logger;
        private readonly IMongoCollection<MobileVersion> _mobileVersion;
        private readonly MobileConfig _mobileConfig;

        public MobileVersionServices(
            ILogger<MobileVersionServices> logger,
            IMongoDbConnection connection,
            IOptions<MobileConfig> mobileConfigOption)
        {
            _logger = logger;
            var client = new MongoClient(connection.ConnectionString);
            var database = client.GetDatabase(connection.DataBase);
            _mobileVersion = database.GetCollection<MobileVersion>(Common.MongoCollection.MobileVersion);
            _mobileConfig = mobileConfigOption.Value;
        }
        public MobileVersion Create(MobileVersion mobileVersion)
        {
            try
            {
                _mobileVersion.InsertOne(mobileVersion);
                return mobileVersion;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }
        public List<MobileVersion> GetMobileVersions()
        {
            var lstMobileVersion = new List<MobileVersion>();
            try
            {
                lstMobileVersion = _mobileVersion.Find(m => true).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return lstMobileVersion;
        }
        public MobileVersion GetMobileVersion(string type, string version)
        {
            var objMobileVersion = new MobileVersion();
            try
            {
                objMobileVersion = _mobileVersion.Find(m => m.Type.ToLower() == type.ToLower() && m.Version == version).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return objMobileVersion;
        }
        public string GetMobileVersion(string type)
        {
            return string.Equals(type, $"{OsType.Android}", StringComparison.OrdinalIgnoreCase) ? _mobileConfig.AndroidVersion : _mobileConfig.IOSVersion;
        }
        public bool IsLastedVersion(string osType, string version)
        {
            if (string.Equals(osType, $"{OsType.Web}", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            var mobileVersion = GetMobileVersion(osType);
            if (mobileVersion == null)
            {
                return false;
            }

            Match matchRequestVersion = Regex.Match($"{version}", @"^(\d+)\.(\d+).*$");
            if (!matchRequestVersion.Success)
            {
                return false;
            }

            Match matchDbVersion = Regex.Match($"{mobileVersion}", @"^(\d+)\.(\d+)$");
            if (!matchDbVersion.Success)
            {
                return false;
            }

            if (int.Parse(matchRequestVersion.Groups[1].Value) > int.Parse(matchDbVersion.Groups[1].Value) ||
               int.Parse(matchRequestVersion.Groups[1].Value) == int.Parse(matchDbVersion.Groups[1].Value))
            {
                return true;
            }

            return false;
        }
    }
}
