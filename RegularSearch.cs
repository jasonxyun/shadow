using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class RegularSearch : MonoBehaviour
{
    // Sample JSON for the following script has attached.

    public List<string> thumbnails;
    public float thumbY;
    public List<GameObject> generatedTitle;
    public List<GameObject> generatedSnippet;
    public List<GameObject> generatedThumbnail;
	public List<GameObject> generatedStock;
	public List<GameObject> generatedOnSale;
	public List<GameObject> generatedSalePrice;
	public List<GameObject> generatedOfferType;

     IEnumerator Start ()
	{
        GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color((float)1, (float)1, (float)1));
		string url = "https://api.walmartlabs.com/v1/search?";
		string search = "query=cocacola";
		string cx = "&format=json&"; 
		string key = "apiKey=f6vc2bz4qf4nwf6euezgwar7";
		int count = 5;
		WWW www = new WWW (url + search + cx + key);
		yield return www;

		if (www.error == null) {
			var N = JSON.Parse (www.text);
			List<string> links = new List<string> ();
            List<string> titles = new List<string>();
            List<string> snippet = new List<string>();
			List<string> stock = new List<string>();
			List<string> onSale = new List<string>();
			List<string> offerType = new List<string>();
			List<string> salePrice = new List<string>();
            thumbnails = new List<string>();
            generatedTitle = new List<GameObject>();
            generatedSnippet = new List<GameObject>();
            generatedThumbnail = new List<GameObject>();
			generatedOnSale = new List<GameObject>();
			generatedStock = new List<GameObject>();
            for (int i = 0; i < count; i++) {
				links.Add(N["items"][i]["msrp"].Value);
                titles.Add(N["items"][i]["title"].Value);
                snippet.Add(N["items"][i]["name"].Value);
				onSale.Add("On Sale: " + N["items"][i]["shippingPassEligible"].Value);
				stock.Add(N["items"][i]["stock"].Value);
				salePrice.Add(N["items"][i]["salePrice"].Value);
                offerType.Add(N["items"][i]["offerType"].Value);
				if (N["items"][i]["mediumImage"] != null)
                {
                    thumbnails.Add(N["items"][i]["mediumImage"].Value);
                }
				else if(N["items"][i]["mediumImage"] == null)
                {
                    thumbnails.Add(null);
                }
            }

			float currX = (float)0.63;
			float currY = (float)0.009;
			float currZ = (float)0.700;
            //float currY = (float)4.4;

            float thumbZ = (float)0.530;



            for(int i =0; i<count; i++)
            {
                var theText = new GameObject();
                var textMesh = theText.AddComponent<TextMesh>();
                var meshRenderer = theText.AddComponent<MeshRenderer>();


                textMesh.color = Color.blue;

                textMesh.fontSize = 200;
                theText.transform.position = new Vector3(currX, currY, currZ);
                theText.transform.rotation = Quaternion.Euler((float)90, (float)0, (float)0);
                theText.transform.localScale = new Vector3((float)0.1, (float)0.1, (float)0.1);
                textMesh.text = titles[i];
                generatedTitle.Add(theText);

                if (thumbnails[i] != null)
                {
                    GameObject thumbs = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    thumbs.transform.position = new Vector3((float)0.725, currY, thumbZ);
                   
                    thumbs.transform.localScale = new Vector3((float)0.2, (float)0.2, (float)0.01);

                    WWW img = new WWW(thumbnails[i]);
                    yield return img;
                    thumbs.GetComponent<Renderer>().material.mainTexture = img.texture;
                    generatedThumbnail.Add(thumbs);
					thumbs.transform.rotation = Quaternion.Euler((float)270, (float)90, (float)90);
                    thumbZ -= (float)0.28;
                }
                else
                {
                    //thumbY -= (float)0.559;
                }

                //currZ -= theText.transform.lossyScale.y * 2;

                //TITLES
                GameObject snippets = new GameObject();
                var snipMesh = snippets.AddComponent<TextMesh>();
                var snipRender = snippets.AddComponent<MeshRenderer>();
               // snipMesh.fontSize = 200;
                snippets.transform.position = new Vector3(currX, currY, currZ);
                snippets.transform.rotation = Quaternion.Euler((float)90, (float)0, (float)0);
                snippets.transform.localScale = new Vector3((float)0.04, (float)0.04, (float)0.04);

                generatedSnippet.Add(snippets);

                snipMesh.text = snippet[i];
                snipMesh.color = Color.black;

                currZ -= snipMesh.transform.lossyScale.y * 2;


                //IN STOCK
				GameObject stocks = new GameObject();
				var stockMesh = stocks.AddComponent<TextMesh>();
				var stockRender = stocks.AddComponent<MeshRenderer>();
				stocks.transform.position = new Vector3((float)0.96, currY, currZ);
				stocks.transform.rotation = Quaternion.Euler((float)90, (float)0, (float)0);
				stocks.transform.localScale = new Vector3((float)0.04, (float)0.04, (float)0.04);

				generatedStock.Add(stocks);

				stockMesh.text = stock[i];
				stockMesh.color = Color.black;

				//SALE PRICE
				GameObject salePrices = new GameObject();
				var salePriceMesh = salePrices.AddComponent<TextMesh>();
				var salePriceRender = salePrices.AddComponent<MeshRenderer>();
				salePrices.transform.position = new Vector3((float)1.8, currY, currZ);
				salePrices.transform.rotation = Quaternion.Euler((float)90, (float)0, (float)0);
				salePrices.transform.localScale = new Vector3((float)0.08, (float)0.08, (float)0.08);

				generatedSalePrice.Add(salePrices);

				salePriceMesh.text = "$ " + salePrice[i];
				salePriceMesh.color = Color.green;

				currZ -= stockMesh.transform.lossyScale.y * 3;

				//ON SALE
				GameObject onSales = new GameObject();
				var onSaleMesh = onSales.AddComponent<TextMesh>();
				var onSaleRender = onSales.AddComponent<MeshRenderer>();
				onSales.transform.position = new Vector3((float)1.8, currY, currZ);
				onSales.transform.rotation = Quaternion.Euler((float)90, (float)0, (float)0);
				onSales.transform.localScale = new Vector3((float)0.04, (float)0.04, (float)0.04);

				generatedOnSale.Add(onSales);

				onSaleMesh.text = onSale[i];
				onSaleMesh.color = Color.red;


				//TYPE
				GameObject type = new GameObject();
				var typeMesh = type.AddComponent<TextMesh>();
				var typeRender = type.AddComponent<MeshRenderer>();
				type.transform.position = new Vector3((float)0.96, currY, currZ);
				type.transform.rotation = Quaternion.Euler((float)90, (float)0, (float)0);
				type.transform.localScale = new Vector3((float)0.04, (float)0.04, (float)0.04);


				generatedOfferType.Add(onSales);

				typeMesh.text = offerType[i];
				typeMesh.color = Color.black;
				currZ -= typeMesh.transform.lossyScale.y * 2;



				//saleY -= onSaleMesh.transform.lossyScale.y * 9;




				//currZ -= salePriceMesh.transform.lossyScale.y * 3;
				//saleY -= onSaleMesh.transform.lossyScale.y * 9;



				//currZ -= offerTypeMesh.transform.lossyScale.y * 3;
				//saleY -= onSaleMesh.transform.lossyScale.y * 9;

            }
            /*
            for(int i = 0; i<thumbnails.Count; i++)
            {
           GameObject thumbs = GameObject.CreatePrimitive(PrimitiveType.Cube);
              thumbs.transform.position = new Vector3((float)4.473, currY, (float)12.67);
                thumbs.transform.rotation = Quaternion.Euler(0, (float)-54.166, 0);
                thumbs.transform.localScale = new Vector3((float)0.5, (float)0.5, (float)0.005);
            }
            */
         }
         else
         {
             Debug.Log("ERROR: " + www.error);
         }
     }


    public void generateThumbnails(int index, float yPos)
    {
        if (thumbnails[index] != null)
        {
            GameObject thumbs = GameObject.CreatePrimitive(PrimitiveType.Cube);
            thumbs.transform.position = new Vector3((float)4.473, yPos, (float)12.67);
            thumbs.transform.rotation = Quaternion.Euler(0, (float)-54.166, 0);
            thumbs.transform.localScale = new Vector3((float)0.5, (float)0.5, (float)0.005);
        }
        else
        {
            thumbY -= (float)0.559;
        }
    }

    public void NewStuff(String variable)
    {
        StartCoroutine(NewWord(variable));
    }

    public IEnumerator NewWord(string term)
    {
        destroyObjects(generatedTitle, generatedSnippet, generatedThumbnail);
        GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color((float)1, (float)1, (float)1));
		string url = "http://api.walmartlabs.com/v1/search?";
		string search = "query=ipod";
		string cx = "&format=json&"; 
		string key = "apiKey=f6vc2bz4qf4nwf6euezgwar7";
		int count = 5;
        WWW www = new WWW(url + search + "&" + cx + "&" + "num=" + count + "&" + key);
        yield return www;
        if (www.error == null)
        {
            var N = JSON.Parse(www.text);
            List<string> links = new List<string>();
            List<string> titles = new List<string>();
            List<string> snippet = new List<string>();
            thumbnails = new List<string>();
			for (int i = 0; i < count; i++) {
				links.Add(N["items"][i]["msrp"].Value);
				titles.Add(N["items"][i]["title"].Value);
				snippet.Add(N["items"][i]["name"].Value);
				if (N["items"][i]["thumbnailImage"] != null)
				{
					thumbnails.Add(N["items"][i]["mediumImage"].Value);
				}
				else if(N["items"][i]["mediumImage"] == null)
				{
					thumbnails.Add(null);
				}
			}

            float currX = this.transform.position.x;
            //float currY = this.transform.position.y;
            float currZ = this.transform.position.z;

            float currY = (float)4.4;

            thumbY = (float)4.095;



            for (int i = 0; i < count; i++)
            {
                var theText = new GameObject();
                var textMesh = theText.AddComponent<TextMesh>();
                var meshRenderer = theText.AddComponent<MeshRenderer>();


                textMesh.color = Color.blue;

                textMesh.fontSize = 20;
                theText.transform.position = new Vector3((float)4.28, currY, (float)12.22);
                theText.transform.rotation = Quaternion.Euler((float)0, (float)125, (float)0);
                theText.transform.localScale = new Vector3((float)0.1, (float)0.1, (float)0.1);
                textMesh.text = titles[i];

                generatedTitle.Add(theText);


                if (thumbnails[i] != null)
                {
                    GameObject thumbs = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    thumbs.transform.position = new Vector3((float)4.473, thumbY, (float)12.67);
                    thumbs.transform.rotation = Quaternion.Euler(0, (float)-54.166, 0);
                    thumbs.transform.localScale = new Vector3((float)0.5, (float)0.5, (float)0.005);

                    WWW img = new WWW(thumbnails[i]);
                    yield return img;
                    thumbs.GetComponent<Renderer>().material.mainTexture = img.texture;
                    generatedThumbnail.Add(thumbs);
                }
                else
                {
                    thumbY -= (float)0.559;
                }

                currY -= theText.transform.lossyScale.y * 2;

                GameObject snippets = new GameObject();
                var snipMesh = snippets.AddComponent<TextMesh>();
                var snipRender = snippets.AddComponent<MeshRenderer>();
                snippets.transform.position = new Vector3((float)4.28, currY, (float)12.22);
                snippets.transform.rotation = Quaternion.Euler((float)0, (float)125, (float)0);
                snippets.transform.localScale = new Vector3((float)0.1, (float)0.1, (float)0.1);

                snipMesh.text = snippet[i];
                snipMesh.color = Color.black;
                generatedSnippet.Add(snippets);

                currY -= snipMesh.transform.lossyScale.y * 4;

            }
            /*
            for(int i = 0; i<thumbnails.Count; i++)
            {
           GameObject thumbs = GameObject.CreatePrimitive(PrimitiveType.Cube);
              thumbs.transform.position = new Vector3((float)4.473, currY, (float)12.67);
                thumbs.transform.rotation = Quaternion.Euler(0, (float)-54.166, 0);
                thumbs.transform.localScale = new Vector3((float)0.5, (float)0.5, (float)0.005);
            }
            */
        }
        else
        {
            Debug.Log("ERROR: " + www.error);
        }
    }

    public void destroyObjects(List<GameObject> t, List<GameObject> snip, List<GameObject> thu)
    {
        for(int i = 0; i<t.Count; i++)
        {
            Destroy(t[i]);
        }
        t.Clear();
        for(int i = 0; i<snip.Count; i++)
        {
            Destroy(snip[i]);
        }
        snip.Clear();
        for(int i = 0; i<thu.Count; i++)
        {
            Destroy(thu[i]);
        }
        thu.Clear();
    }
} 
