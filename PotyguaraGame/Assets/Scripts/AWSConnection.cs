using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System.IO;

public class AWSConnection : MonoBehaviour
{
    [SerializeField] private string bucketName = "potyguarabucket";
    [SerializeField] private string target = "meditationClasses/";

    private AmazonS3Client awsS3Client;

    private string localPath = "C:/UnityProject/DownloadedFiles/";
    // Start is called before the first frame update
    void Start()
    {
        //localPath = Path.Combine(Application.persistentDataPath, "MeditationClasses");
        awsS3Client = new AmazonS3Client("AAAAB3NzaC1yc2EAAAADAQABAAABgQDpzTugWnqA5xTOC06Ko686XH5bLllqdKI3EDquQ8nNjK6axVrWXFEc59Phqe0iu+NMqPxhq+IbXZlppre5GNu563llYAK/d/GQXXoG/SQcRn/LY6ij5Y69TLBEmz3zY2vT4Dck24dp6SITkYCKTE66MlS+B5+TVnCRkrpvlreE4gEbXnQFT2AbZlD7OV8aamq0qfaP/aTWE" +
            "SlnmTacDZqUNKygwWMcdbwrVae1mVqp+CIMc4s75Zns5ql/4s3c0dPcEEzao5oUCypsmf7fw7mK4jj6/zSsnk/4m6biCbBP38Qie7/NOGCIU3J1VeN5dsoosdk3oRNuuJWXnF/jSzYB3pEhmHZbS5nbo4fDXT+TkLzhmmBoAqHJYjfT1uLwlhBe4mQayUl28/IyzMsBW66MN55nrCs0fOnXuQwHsj9gRnQKojCPcoTtSm5yochVVeYwwjeWguEjJ" +
            "PrW1evHUENocb3FfKkRVapxg+YmlyuP8+FZjXxQ8TgZIT3jv5TDXD0=", "b3BlbnNzaC1rZXktdjEAAAAABG5vbmUAAAAEbm9uZQAAAAAAAAABAAABlwAAAAdzc2gtcnNhAAAAAwEAAQAAAYEA6c07oFp6gOcUzgtOiqOvOlx+Wy5ZanSiNxA6rkPJzYyumsVa1lxRHOfT4antIrvjTKj8YaviG12Zaaa3uRjbu" +
            "et5ZWACv3fxkF16Bv0kHEZ/y2Ooo+WOvUywRJs982Nr0+A3JNuHaekiE5GAikxOujJUvgefk1ZwkZK6b5a3hOIBG150BU9gG2ZQ+zlfGmpqtKn2j/2k1hEpZ5k2nA2alDSsoMFjHHW8K1WntZlaqfgiDHOLO+WZ7Oapf+LN3NHT3BBM2qOaFAsqbJn+38O5iuI4+v80rJ5P+Jum4gmwT9/EInu/zThgiFNydVXjeX" +
            "bKKLHZN6ETbriVl5xf40s2Ad6RIZh2W0uZ26OHw10/k5C84ZpgaAKhyWI309bi8JYQXuJkGslJdvPyMszLAVuujDeeZ6wrNHzp17kMB7I/YEZ0CqIwj3KE7UpucqHIVVXmMMI3loLhIyT61tXrx1BDaHG9xXypEVWqcYPmJpcrj/PhWY18UPE4GSE947+Uw1w9AAAFoJ4DZtieA2bYAAAAB3NzaC1yc2EAAAGBAOn" +
            "NO6BaeoDnFM4LToqjrzpcflsuWWp0ojcQOq5Dyc2MrprFWtZcURzn0+Gp7SK740yo/GGr4htdmWmmt7kY27nreWVgAr938ZBdegb9JBxGf8tjqKPljr1MsESbPfNja9PgNyTbh2npIhORgIpMTroyVL4Hn5NWcJGSum+Wt4TiARtedAVPYBtmUPs5XxpqarSp9o/9pNYRKWeZNpwNmpQ0rKDBYxx1vCtVp7WZWqn4Igxz" +
            "izvlmezmqX/izdzR09wQTNqjmhQLKmyZ/t/DuYriOPr/NKyeT/ibpuIJsE/fxCJ7v804YIhTcnVV43l2yiix2TehE264lZecX+NLNgHekSGYdltLmdujh8NdP5OQvOGaYGgCocliN9PW4vCWEF7iZBrJSXbz8jLMywFbrow3nmesKzR86de5DAeyP2BGdAqiMI9yhO1KbnKhyFVV5jDCN5aC4SMk+tbV68dQQ2hxvcV8q" +
            "RFVqnGD5iaXK4/z4VmNfFDxOBkhPeO/lMNcPQAAAAMBAAEAAAGAUHfOedYqK4tc4b2KfrTvbkq/QmqlY7pYylLPn4K9Rf2RartaFEUZtbUke5qcf0Pja1MN6h/aZkjEsFQtD4u6tDaTRYzR5UxG28UQZq9haknpWfsm46HyiryGPlaf79DcH8QftpPH+2+9xrZhYzf4MKV5/R1qkq9BdxjlCdr5tsaCUwUjMWCsV" +
            "0HTxWNtlA3kQ+Hrdot53CNz+LAG15bnCPNIJtvoXLc42EFWw23QgodgUKDD3w2a+Ko61EUfTUz50WvHU90OlGfi/gd+65fWnvWoGE3iwqBw1PhrSLOOlzwgabnPF9D71HPhh531z8vMdHRWHa1Q/YXyiU8M3Kt9MdKev/sX+YyO6O87F3YotQMmO2NAXBmRYeKvExqDwDYDKCl4ienP9wsMd7iAyXwpSHiM8b8TiW8gQ" +
            "P5koooPEDjZuxdMqkP/UKEvJQlWQdtG9vFfoeAWPwahoS0vaDt9hAz7lTXYSswNI81i2Y/BJusnASRaT6pSeKD3IS1BNj4BAAAAwQDtMnNFoTWjNDP58UHzHsPnQ+pP/0ZGTRRGKyrd2lCaQVc+hMu8JZUnmaoDiPeKkR0i0YSjbWBiz1iu7yS5wnlMyc6fK0Bw89gHghzS4hhB2pxsWpwDu8+eiqQ4aIju2fKaIKbw" +
            "TD0lUXoVrsN34DZ6RGyVmof7nAlyF0p7D8f03nnVzTSnFb/1gWsE4ilZ+7bqJ7VacrkuBI4p54C0NEDEw6Bs1dGRCnMDnYYORCBhLaCzE2FvU34zTjq65Ql7egUAAADBAPgg8pbbD22YYd9ZItrizrvpN+Mm3Px2Om+u3z1yXfC7Qrkf2GeyyCj8tPmSPT85HHIvSTyQ0GKTq1jnAiZRaJ95JTg6ivpqxA4slVE" +
            "SrTLDqstXCvmhfv3V3UC8rNEynwfQKtjESPwPtxEyhT7AQJFmpObLTNQT/fWk/iZunkArpOx9Nb4+TxBPebDftn9qCpg0di/xGnku0K65UmQjcKmWTEXD7ExlozaE4s1UNqGhCKF/cuF+kuN8tT7BGrLL4QAAAMEA8TfvibEt4p5IGUc/8152mlpk2yAVCbDUU7/g04AhoLZIanf+/DKFSfWoD61XttcqsOrEcCbXA1" +
            "lZY1OlD9LIvSYs6jVhIkSpEYma+qcumjs3DsFoO70YfvX7/XO/3/4fDdY+qtEuacmiG7CaRNf1k6alw2LrcgocaY/Kf6CtZwR/FN8dZI8IuviwURMRZAugjZvj9QgsRmS/Pw0cngYIjCofWUxyIKNOXvfbAG7qr6AXoy3SXgH9BXtQpPG8/LvdAAAAJ21hcmluYWphdWR5QE1hY0Jvb2stUHJvLWRlLU1hcmluYS5sb" +
            "2NhbAECAw==", RegionEndpoint.USEast1);

        StartCoroutine(DownloadFolderFromS3());
    }

    public IEnumerator DownloadFolderFromS3()
    {
        var request = new ListObjectsV2Request
        {
            BucketName = bucketName,
            Prefix = target,
        };

        var listTask = awsS3Client.ListObjectsV2Async(request);


        yield return new WaitUntil(() => listTask.IsCompleted);

        Debug.Log("S3:" + listTask.Result);

        foreach (var file in listTask.Result.S3Objects)
        {
            if (!file.Key.EndsWith("/"))
                StartCoroutine(DownloadFile(file.Key));
        }
    }

    public IEnumerator DownloadFile(string fileKey)
    {
        string filePath = Path.Combine(localPath, Path.GetFileName(fileKey));

        GetObjectRequest request = new GetObjectRequest
        {
            BucketName = bucketName,
            Key = fileKey
        };

        var task = awsS3Client.GetObjectAsync(request);
        yield return new WaitUntil(() => task.IsCompleted);

        using (Stream responseStream = task.Result.ResponseStream)
        using (FileStream fileStream = File.Create(filePath))
        {
            responseStream.CopyTo(fileStream);
        }

        Debug.Log("Arquivos baixado: " + filePath);
    }

}
