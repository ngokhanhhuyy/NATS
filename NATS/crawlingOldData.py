from __future__ import annotations
import mysql.connector
from dataclasses import dataclass
import json
from typing import *
import devtools
from datetime import datetime
import requests
import base64

url = "https://nats.khanhhuy.dev/"

class DtoJsonEncoder(json.JSONEncoder):
    def default(self, obj):
        if isinstance(obj, Dto):
            return obj.__dict__
        return json.JSONEncoder.default(self, obj)

class Dto:
    pass

@dataclass
class Product(Dto):
    id: int
    name: str
    summary: str
    detail: str
    thumbnail_url: str | None
    features: list[ProductFeature]
    photos: list[ProductPhoto]

@dataclass
class ProductFeature(Dto):
    id: int
    content: str
    product_id: int

@dataclass
class ProductPhoto(Dto):
    id: int
    url: str
    product_id: int

def fetchOldData() -> List[Product]:
    connection = mysql.connector.connect(
        host ="mysql8003.site4now.net",
        database="db_aa5821_nats",
        user ="aa5821_nats",
        passwd ="Huyy47b1")
    cursor = connection.cursor(dictionary=True)

    products: List[Product] = []
    cursor.execute("SELECT * FROM products")
    product_rows = cursor.fetchall()

    for row in product_rows:
        products.append(Product(
            id = row["id"],
            name = row["name"],
            summary = row["summary"],
            detail = row["detail"],
            thumbnail_url = row["thumbnail_url"],
            features = [],
            photos = []))

    cursor.execute("SELECT * FROM product_features")
    product_feature_rows = cursor.fetchall()
    for row in product_feature_rows:
        product_feature = ProductFeature(
            id = row["id"],
            content = row["content"],
            product_id = row["product_id"])
        product = next((p for p in products if p.id == product_feature.product_id))
        product.features.append(product_feature)
        
    cursor.execute("SELECT * FROM product_photos")
    product_photo_rows = cursor.fetchall()
    for row in product_photo_rows:
        product_photo = ProductPhoto(
            id = row["id"],
            url = row["url"],
            product_id = row["product_id"])
        product = next((p for p in products if p.id == product_photo.product_id))
        product.photos.append(product_photo)

    data_as_json = json.dumps(products, indent=2, cls=DtoJsonEncoder)
    print([p.id for p in products])

    with open("oldData.json", "w") as file:
        file.write(data_as_json)

    connection.close()
    return products

def insertOldDataToNewDatabase(products: List[Product]):
    connection = mysql.connector.connect(
        host ="localhost",
        database="nats",
        user ="root",
        passwd ="huy123")
    cursor = connection.cursor(dictionary=True)
    product_data = []
    product_feature_data = []
    product_photo_data = []
    for product in products:
        product_data.append((
            product.name,
            product.summary,
            product.detail,
            product.thumbnail_url))
    cursor.executemany(
        """
            INSERT INTO products (name, summary, detail, thumbnail_url)
            VALUES (%s, %s, %s, %s)
        """, product_data)
    cursor.execute("SELECT id FROM products ORDER BY id")
    rows = cursor.fetchall()
    for index in range(len(rows)):
        product = products[index]
        product.id = rows[index]["id"]
        for feature in product.features:
            feature.product_id = product.id
            product_feature_data.append((feature.content, feature.product_id))
        for photo in product.photos:
            photo.product_id = product.id
            product_photo_data.append((photo.url, photo.product_id))
    
    cursor.executemany(
        "INSERT INTO product_features (content, product_id) VALUES (%s, %s)",
        product_feature_data)
    cursor.executemany(
        "INSERT INTO product_photos (url, product_id) VALUES (%s, %s)",
        product_photo_data)
    
    connection.commit()
    connection.close()


insertOldDataToNewDatabase(fetchOldData())


