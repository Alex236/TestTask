var productsContainer = document.getElementById("productsContainer");
var clientProductCounter = 0;

class Product {
    constructor(id, url, imageUrl, name) {
        this.id = id;
        this.url = url;
        this.imageUrl = imageUrl;
        this.name = name;
    }

    createNode() {
        var node = document.createElement("div");
        node.id = this.id;
        var name = document.createElement("h4");
        name.innerText = this.name;
        var image = document.createElement("img");
        image.setAttribute("src", this.imageUrl);
        var link = document.createElement("a");
        link.innerText = "link to market";
        link.setAttribute("href", this.url);
        productsContainer.appendChild(node);
        node.appendChild(name);
        node.appendChild(link);
        node.appendChild(image);
        node.addEventListener("click", function (event) {
            connection.invoke("GetPrices", node.id).catch(function (err) {
                return console.error(err.toString());
            });
        });
    }
}

document.getElementById("LoadMore").addEventListener("click", function (event) {
    connection.invoke("GetProducts", clientProductCounter).catch(function (err) {
        return console.error(err.toString());
    });
});

var connection = new signalR.HubConnectionBuilder().withUrl("/priceChecker").build();

connection.start().catch(function (err) {
    return console.error(err.toString());
});

connection.on("getProducts", function (products) {
    if (products == "[]") {
        document.getElementById("LoadMore").hidden = true;
    }
    else {
        var productList = JSON.parse(products);
        productList.forEach(product => {
            var instance = new Product(product.Id, product.Url, product.ImageUrl, product.Name);
            instance.createNode();
            clientProductCounter++;
        });
    }
});

connection.on("getPrices", function (prices) {
    var parsedPrices;
    try {
        document.getElementById("shedule").remove();
    }
    catch {

    }
    parsedPrices = JSON.parse(prices);
    if (parsedPrices.lenght != 0) {
        google.charts.load('current', { 'packages': ['corechart'] });
        google.charts.setOnLoadCallback(drawChart);
    }
    function drawChart() {
        var list = [];
        list.push([date = "Date", _price = "Price"]);
        parsedPrices.forEach(price => list.push([date = price.Time, _price = price.Price]));
        var data = google.visualization.arrayToDataTable(list);

        var options = {
            vAxis: { minValue: 0 }
        };
        var shedule = document.createElement("div");
        shedule.setAttribute("id", "shedule");
        shedule.setAttribute("style", "width: 40%; height: 500px");
        document.getElementById(parsedPrices[0].ProductId).appendChild(shedule);
        var chart = new google.visualization.AreaChart(shedule);
        chart.draw(data, options);
    }
});
