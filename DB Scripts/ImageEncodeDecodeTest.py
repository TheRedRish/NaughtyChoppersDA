from PIL import Image
import io
import pyodbc



# Encoding
def encode_image_to_byte_array(image_path):
    image = Image.open(image_path)
    image_byte_array = io.BytesIO()
    image.save(image_byte_array, format=image.format)
    return image_byte_array.getvalue()

connection_string = (
    "Driver={SQL Server};"
    "Server=.;"
    "Database=NaughtyChoppersDB;"
    "Trusted_Connection=yes;"
)

connection = pyodbc.connect(connection_string)
cursor = connection.cursor()

select_query = (f"SELECT ProfileImg FROM ProfileInformation WHERE Id = '04B6F9F5-0EA2-4FEF-ACE4-1BFE1AF9D3D0'")

cursor.execute(select_query)
encoded_image_bytes = cursor.fetchall()[0]

# Decoding
def decode_byte_array_to_image(byte_array):
    image = Image.open(io.BytesIO(byte_array))
    return image

# Call the function with your encoded image bytes
decoded_image = decode_byte_array_to_image(encoded_image_bytes[0])

# Display or save the decoded image as needed
decoded_image.show()
# or decoded_image.save("decoded_image.png")