using MarketSpot.Models;
using MarketSpot.Models.Interface;
using MarketSpot.Models.Services;
using Microsoft.Azure.WebJobs;

namespace MarketSpot
{
    /// <summary>�}�[�P�b�g�t�@���N�V����</summary>
    public class MarketFunction
    {
        /// <summary>�ݒ�</summary>
        private readonly Settings Config;

        /// <summary>�o�C�i���X�T�[�r�X</summary>
        private readonly BinanceService Binance;

        /// <summary>CSV</summary>
        private readonly ICSVManipulator CSV;

        /// <summary>�בփ��[�g</summary>
        private readonly IExchengerates Excheangerates;

        /// <summary>�R���X�g���N�^</summary>
        /// <param name="config">�ݒ�(DI)</param>
        /// <param name="binance">�o�C�i���X�T�[�r�X(DI)</param>
        /// <param name="csv">CSV(DI)</param>
        public MarketFunction(Settings config, BinanceService binance, ICSVManipulator csv, IExchengerates exchengerates)
        {
            Config = config;
            Binance = binance;
            CSV = csv;
            Excheangerates = exchengerates;
        }

        /// <summary>�T�}�����|�[�g���e</summary>
        /// <param name="myTimer">myTimer</param>
        [FunctionName("ReportBinance")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:���g�p�̃p�����[�^�[���폜���܂�", Justification = "�^�C�}�[�ݒ�̂���")]
        public void Run([TimerTrigger("0 */15 * * * *")] TimerInfo myTimer)
        {
            Binance.Report(CSV.ReadSymbolList(Config.CsvPath), Excheangerates.GetJPYRate());
        }
    }
}